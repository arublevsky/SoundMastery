// profile.tsx
import React from 'react';
import { View, StyleSheet } from 'react-native';
import { Avatar, Button, Title, Caption } from 'react-native-paper';
import { useAuthContext } from '../../modules/authorization/context.ts';
import { formatFullName } from '../utils.ts';

const Profile = () => {
  const { userProfile } = useAuthContext();

  const handleEditProfile = () => {
    // Handle the edit profile action here
  };

  return (
    <View style={styles.container}>
      <Avatar.Image source={{ uri: userProfile.avatarUrl }} size={100} style={styles.avatar} />
      <Title style={styles.name}>{formatFullName(userProfile?.user!)}</Title>
      <Caption style={styles.email}>{userProfile?.user.email}</Caption>
      <Caption style={styles.role}>{userProfile?.isTeacher ? "Teacher" : "Student"}</Caption>
      <Button mode="contained" onPress={handleEditProfile} style={styles.button}>
        Edit Profile
      </Button>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    alignItems: 'center',
    padding: 16,
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
});

export default Profile;