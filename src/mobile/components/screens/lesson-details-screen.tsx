import React, { useState } from 'react';
import { StyleSheet, View, Share, ActionSheetIOS, Linking, ScrollView } from 'react-native';
import { ScreenProps } from '../types';
import { Button, Card, List, Modal, TextInput, Title } from 'react-native-paper';
import { LessonMaterial, cancel, complete, addMaterial, getLessonMaterials } from '../../modules/api/lessonsApi';
import { useErrorHandling } from '../../modules/errors/useErrorHandling';
import { showComfirmationAlert, showErrorAlert, showSuccessAlert } from '../common';
import { v4 as uuidv4 } from 'uuid';
import LessonCardContent from '../tabs/my-lessons/my-lesson-card-content';
import ImagePicker from 'react-native-image-crop-picker';
import { getDownloadUri, upload } from '../../modules/api/fileApi';
import RNFS from 'react-native-fs';
import { getAuthHeader } from '../../modules/common/requestApi';
import DocumentPicker from 'react-native-document-picker'
import { isValidUrl } from '../utils';

const MaterialTypes = {
    cancel: { text: 'Cancel', index: 0 },
    file: { text: 'File', index: 1 },
    photo: { text: 'Photo', index: 2 },
    link: { text: 'Web Link', index: 3 },
};

const LessonDetailsScreen = ({ route, navigation }: ScreenProps<'LessonDetailsScreen'>) => {
    const { lesson, isTeacher } = route.params;
    const editable = !lesson.cancelled && !lesson.completed;

    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const [materials, setMaterials] = useState(lesson.materials || []);
    const [modalType, setModalType] = useState(0);
    const [description, setDescription] = useState('');
    const [url, setUrl] = useState('');

    const handleCancel = () => showComfirmationAlert(
        "Cancel Lesson",
        "Are you sure you want to cancel this lesson?",
        cancelLesson);

    const handleComplete = () => showComfirmationAlert(
        "Complete Lesson",
        "Are you sure you want to complete this lesson?",
        completeLesson);


    const cancelLesson = () => asyncHandler(async () => {
        await cancel(lesson.id);
        onUpdateSuccess('Lesson cancelled successfully');
    });

    const completeLesson = () => asyncHandler(async () => {
        await complete(lesson.id);
        onUpdateSuccess('Lesson completed successfully');
    });

    if (errors.length) {
        const error = errors[0];
        if (error.code === "E_PICKER_CANCELLED" || error.code === "DOCUMENT_PICKER_CANCELED") {
            clearErrors();
            setModalType(0);
            return;
        }

        showErrorAlert(`Error occured: ${error.description} (${error.code}).`, clearErrors);
    }

    const onUpdateSuccess = (message: string) => {
        showSuccessAlert(message, () => {
            clearErrors();
            navigation.navigate("MyLessons", { refreshToken: uuidv4() })
        });
    };

    const renderMaterial = (material: LessonMaterial, i: number) => {
        return (
            <List.Item
                key={i}
                title={material.description}
                description={material.file?.fileName || material.url}
                right={props => <List.Icon {...props} icon={material.file ? "file" : "link"} />}
                onPress={() => handleMaterialClick(material)}
            />)
    }

    const handleAddMaterial = () => {
        ActionSheetIOS.showActionSheetWithOptions(
            {
                options: Object.values(MaterialTypes).map(t => t.text),
                cancelButtonIndex: MaterialTypes.cancel.index,
            },
            (index) => setModalType(index),
        );
    };

    const handleMaterialClick = (material: LessonMaterial) => asyncHandler(async () => {
        if (material.url) {
            if (!await Linking.canOpenURL(material.url)) {
                showErrorAlert(`Don't know how to open this URL: ${material.url}`);
                return;
            }

            await Linking.openURL(material.url);
            return;
        }

        const path = `${RNFS.DocumentDirectoryPath}/${material.file!.fileName}`;
        const downloadOptions = {
            fromUrl: getDownloadUri(material.file!.fileId!, false),
            toFile: path,
            headers: {
                ...getAuthHeader()
            }
        };

        const result = await RNFS.downloadFile(downloadOptions).promise;

        if (result.statusCode === 200) {
            await Share.share({ url: path });
        }
        else {
            showErrorAlert(`Failed to download file: status ${result.statusCode}.`);
        }
    });

    const handleSubmitModal = () => asyncHandler(async () => {
        switch (modalType) {
            case MaterialTypes.file.index:
                await handleUploadFile();
                break;
            case MaterialTypes.photo.index:
                await handleUploadPhoto();
                break;
            case MaterialTypes.link.index:
                await handleAddLink();
                break;
            default:
                throw new Error(`Unknown modal type: ${modalType}`);
        }

        onModalDismiss();
    });

    const onModalDismiss = () => {
        setModalType(0);
        setDescription('');
        setUrl('');
    };

    const handleUploadFile = async () => {
        const file = await DocumentPicker.pickSingle({ type: [DocumentPicker.types.allFiles] });
        await addFileMaterial(file.uri, file.type, file.name);
    }

    const handleUploadPhoto = async () => {
        const image = await ImagePicker.openPicker({ cropping: false });
        await addFileMaterial(image.path, image.mime, image.filename);
    }

    const handleAddLink = async () => {
        if (!isValidUrl(url)) {
            showErrorAlert("URL must start with http:// or https://");
            return;
        }

        await addMaterial(lesson.id, { description: description, url: url });
        await reloadMaterials();
    }

    const addFileMaterial = async (path: string, mime?: string | null, filename?: string | null) => {
        const fileId = await upload(path, mime || "unknown", filename || "unknown");
        await addMaterial(lesson.id, { description: description, fileId: fileId });
        await reloadMaterials();
    }

    const reloadMaterials = async () => {
        const materials = await getLessonMaterials(lesson.id);
        lesson.materials = materials;
        setMaterials(materials || []);
    };

    return (
        <ScrollView>
            <View style={styles.container}>
                <Card style={styles.card}>
                    <LessonCardContent lesson={lesson} isTeacher={isTeacher} />
                </Card>
                {materials?.length
                    ? <Card style={styles.card}>
                        <Card.Title title={"Lesson Materials"} />
                        <Card.Content>
                            {materials.map((material, i) => renderMaterial(material, i))}
                        </Card.Content>
                    </Card>
                    : null}
                <View style={styles.buttonContainer}>
                    <Button mode="contained" onPress={handleAddMaterial} style={styles.button}>
                        Add Material
                    </Button>
                    <Button mode="contained" onPress={handleComplete} style={styles.button} disabled={!editable}>
                        Complete Lesson
                    </Button>
                    <Button mode="contained" onPress={handleCancel} style={styles.button} disabled={!editable}>
                        Cancel Lesson
                    </Button>
                </View>
                <Modal visible={!!modalType} onDismiss={onModalDismiss} contentContainerStyle={styles.modalContent} style={styles.modal}>
                    <Title style={styles.modalTitle}>Add material</Title>
                    <TextInput
                        label="Description"
                        value={description}
                        onChangeText={setDescription}
                        style={styles.modalInput}
                    />
                    {modalType === 3
                        ? <TextInput label="URL" value={url} onChangeText={setUrl} style={styles.modalInput} />
                        : null}
                    <Button
                        onPress={handleSubmitModal}
                        disabled={!description.length || (modalType === MaterialTypes.link.index && !url.length)}
                        style={styles.modalButton}
                    >
                        Submit
                    </Button>
                </Modal>
            </View>
        </ScrollView>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'space-between',
    },
    button: {
        margin: 8,
    },
    buttonContainer: {
        marginBottom: 16,
    },
    card: {
        margin: 10,
    },
    modalContent: {
        backgroundColor: 'white',
        padding: 20,
        margin: 20,
    },
    modal: {
        justifyContent: 'flex-end',
    },
    modalTitle: {
        marginBottom: 20,
    },
    modalInput: {
        marginBottom: 10,
    },
    modalButton: {
        marginTop: 10,
    },
});

export default LessonDetailsScreen;
