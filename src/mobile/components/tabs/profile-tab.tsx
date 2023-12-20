// profile.tsx
import React, { useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Avatar, Button, Title, Caption } from 'react-native-paper';
import { useAuthContext } from '../../modules/authorization/context.ts';
import { formatFullName } from '../utils.ts';
import ImagePicker from 'react-native-image-crop-picker';
import RNFS from 'react-native-fs';
import { useErrorHandling } from '../../modules/errors/useErrorHandling.tsx';
import { uploadAvatar } from '../../modules/api/profileApi.ts';

const Profile = () => {
    const { userProfile } = useAuthContext();
    const [avatar, setAvatar] = useState(userProfile?.user.avatar);

    const [errors, asyncHandler, clearErrors] = useErrorHandling();

    const avatarUploadOptions = {
        width: 300,
        height: 300,
        cropping: true
    };

    const handleEditProfile = () => {
        // Handle the edit profile action here
    };

    const handleUpload = () => asyncHandler(async () => {
        const image = await ImagePicker.openPicker(avatarUploadOptions);
        const base64 = await RNFS.readFile(image.path, 'base64');
        // await uploadAvatar({ image: base64 });
        setAvatar(base64);
    });

    const handleCamera = async () => {
        const image = await ImagePicker.openCamera(avatarUploadOptions);
        console.log(image);
        const base64 = RNFS.readFile(image.path, 'base64')
        console.log(base64);
    };

    return (
        <View style={styles.container}>
            <View style={styles.contentContainer}>
                <Avatar.Image
                    source={{ uri: `data:image/png;base64,${avatar}` }}
                    size={100}
                    style={styles.avatar}
                />
                <Button style={styles.uploadButton} mode="contained" icon="camera" onPress={handleUpload}>
                    Upload photo
                </Button>
                <Button style={styles.uploadButton} mode="contained" icon="camera" onPress={handleCamera}>
                    Take a photo
                </Button>
                <Title style={styles.name}>{formatFullName(userProfile?.user!)}</Title>
                <Caption style={styles.email}>{userProfile?.user.email}</Caption>
                <Caption style={styles.role}>{userProfile?.isTeacher ? "Teacher" : "Student"}</Caption>
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
        marginBottom: 16,
    },
    button: {
        marginTop: 16,
    },
    uploadButton: {
        marginBottom: 16,
    }
});

export default Profile;