import React, { useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Avatar, Button, Title, Caption } from 'react-native-paper';
import { useAuthContext } from '../../modules/authorization/context.ts';
import { formatFullName } from '../utils.ts';
import ImagePicker from 'react-native-image-crop-picker';
import RNFS from 'react-native-fs';
import { useErrorHandling } from '../../modules/errors/useErrorHandling.tsx';
import { uploadAvatar } from '../../modules/api/profileApi.ts';
import { images } from '../../assets/index.ts';
import { showErrorAlert } from '../common.tsx';
import { useNavigation } from '@react-navigation/native';
import { ScreenProps } from '../types.ts';

const avatarUploadOptions = {
    width: 300,
    height: 300,
    cropping: true
};

const ProfileTab = () => {
    const { userProfile } = useAuthContext();
    const [avatar, setAvatar] = useState(userProfile!.user.avatar);
    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const navigator = useNavigation<ScreenProps<'ProfileTab'>['navigation']>();

    const handleEditProfile = () => navigator.navigate('EditProfileScreen');

    const handleUpload = () => asyncHandler(async () => {
        const image = await ImagePicker.openPicker(avatarUploadOptions);
        await onImageSelected(image.path);
    });

    const onImageSelected = async (path: string) => {
        const base64 = await RNFS.readFile(path, 'base64')
        await uploadAvatar({ image: base64 });
        setAvatar(base64);
    }

    if (errors.length) {
        showErrorAlert(`Failed to upload avatar: ${errors[0].description}.`, clearErrors);
    }

    return (
        <View style={styles.container}>
            <View style={styles.contentContainer}>
                <Avatar.Image
                    source={avatar ? { uri: `data:image/png;base64,${avatar}` } : images.avatar}
                    size={100}
                    style={styles.avatar}
                />
                <Button style={styles.uploadButton} mode="contained" icon="camera" onPress={handleUpload}>
                    Upload photo
                </Button>
                <Title style={styles.name}>{formatFullName(userProfile!.user)}</Title>
                <Caption style={styles.email}>{userProfile!.user.email}</Caption>
                <Caption style={styles.role}>{userProfile!.isTeacher ? "Teacher" : "Student"}</Caption>
                {userProfile!.isTeacher && userProfile?.workingHours
                    ? <Caption style={styles.workingHours}>
                        Working Hours: {`${userProfile!.workingHours.from}:00 - ${userProfile?.workingHours.to}:00` }
                    </Caption>
                    : null}
            </View>
            <Button mode="contained" onPress={handleEditProfile} style={styles.button}>
                Edit Profile
            </Button>
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        padding: 16,
        justifyContent: 'space-between',
    },
    contentContainer: {
        flex: 1,
        alignItems: 'center',
    },
    avatar: {
        marginBottom: 16,
    },
    name: {
        fontSize: 24,
        marginBottom: 8,
    },
    email: {
        fontSize: 16,
        marginBottom: 8,
    },
    role: {
        fontSize: 16,
        marginBottom: 8,
    },
    workingHours: {
        fontSize: 16,
        marginBottom: 8,
    },
    button: {
        marginTop: 16,
    },
    uploadButton: {
        marginBottom: 16,
    }
});

export default ProfileTab;