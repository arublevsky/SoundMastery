import React, { useState } from 'react';
import { View, StyleSheet } from 'react-native';
import { Button, Card, Text, TextInput } from 'react-native-paper';
import { useAuthContext } from '../../modules/authorization/context.ts';
import { updateUserProfile } from '../../modules/api/profileApi.ts';
import RNPickerSelect from 'react-native-picker-select';
import { arrayRange } from '../utils.ts';
import { pickerSelectStyles, showErrorAlert, showSuccessAlert } from '../common.tsx';
import { useErrorHandling } from '../../modules/errors/useErrorHandling.tsx';
import { useNavigation } from '@react-navigation/native';
import { ScreenProps } from '../types.ts';

const workingTimeLimit = arrayRange(6, 23, 1);

const EditProfileScreen = () => {
    const { userProfile, updateProfile } = useAuthContext();
    const user = userProfile!.user;
    const userWorkingHours = userProfile!.workingHours;
    const navigator = useNavigation<ScreenProps<'EditProfileScreen'>['navigation']>();

    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const [firstName, setFirstName] = useState(user.firstName);
    const [lastName, setLastName] = useState(user.lastName);
    const [workingHoursFrom, setWorkingHoursFrom] = useState(userWorkingHours?.from.toString());
    const [workingHoursTo, setWorkingHoursTo] = useState(userWorkingHours?.to.toString());

    const handleSave = () => asyncHandler(async () => {
        const newProfile = await updateUserProfile({
            user: {
                ...user,
                firstName,
                lastName
            },
            workingHours: {
                from: Number(workingHoursFrom),
                to: Number(workingHoursTo)
            }
        });

        showSuccessAlert('Profile saved successfully', () => {
            updateProfile(newProfile);
            navigator.navigate("ProfileTab");
        });
    });

    if (errors.length) {
        showErrorAlert(`Failed to save profile: ${errors[0].description}.`, clearErrors);
    }

    return (
        <View style={styles.container}>
            <View>
                <Card style={styles.card}>
                    <Card.Title title="Personal info" />
                    <Card.Content>
                        <TextInput label="First Name" value={firstName} onChangeText={setFirstName} style={styles.input} />
                        <TextInput label="Last Name" value={lastName} onChangeText={setLastName} style={styles.input} />
                    </Card.Content>
                </Card>
                <Card style={styles.card}>
                    <Card.Title title="Working Hours" />
                    <Card.Content>
                        <View style={styles.row} id="working-hours">
                            <Text style={styles.label}>From:</Text>
                            <RNPickerSelect
                                items={workingTimeLimit.map(hour => ({
                                    label: `${hour}:00`,
                                    value: hour.toString()
                                }))}
                                onValueChange={setWorkingHoursFrom}
                                placeholder={{ label: 'Select from time', value: '' }}
                                value={workingHoursFrom}
                                style={pickerSelectStyles}
                            />
                            <Text style={styles.label}>To:</Text>
                            <RNPickerSelect
                                items={workingTimeLimit.map(hour => ({
                                    label: `${hour}:00`,
                                    value: hour.toString()
                                }))}
                                onValueChange={setWorkingHoursTo}
                                placeholder={{ label: 'Select to time', value: '' }}
                                value={workingHoursTo}
                                style={pickerSelectStyles}
                            />
                        </View>
                    </Card.Content>
                </Card>
            </View>
            <Button mode="contained" onPress={handleSave} style={styles.button}>
                Save
            </Button>
        </View >
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        padding: 16,
        justifyContent: 'space-between',
    },
    input: {
        marginBottom: 16,
    },
    button: {
        marginBottom: 16,
    },
    row: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-evenly',
        marginBottom: 16,
    },
    label: {
        marginRight: 8,
    },
    card: {
        marginBottom: 16,
    },
});

export default EditProfileScreen;