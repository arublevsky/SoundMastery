import React, { useState } from 'react';
import {
    StyleSheet,
    Alert,
    Keyboard,
    TouchableWithoutFeedback, Platform, KeyboardAvoidingView
} from 'react-native';
import RNPickerSelect from 'react-native-picker-select';
import { useErrorHandling } from "../../modules/errors/useErrorHandling.tsx";
import { registerUser } from "../../modules/api/accountApi.ts";
import { useAuthContext } from "../../modules/authorization/context.ts";
import { ApplicationError } from "../../modules/common/errorHandling.ts";
import { useNavigation } from "@react-navigation/native";
import { RegisterScreenNavigationProps } from "../types.ts";
import { Button, Card, Menu, TextInput } from 'react-native-paper';

const DefaultRole = { label: 'Select a role...', value: '' };

const RegisterScreen = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [role, setRole] = useState(DefaultRole);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [visible, setVisible] = React.useState(false);

    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const { onLoggedIn } = useAuthContext();
    const navigator = useNavigation<RegisterScreenNavigationProps>();

    const openMenu = () => setVisible(true);
    const closeMenu = () => setVisible(false);

    const handleRegister = () => asyncHandler(async () => {
        if (!email || !firstName || !lastName || !password || !role) {
            Alert.alert('Error', 'Please fill all required fields');
            return;
        }

        const result = await registerUser({ email, firstName, lastName, password, role: Number(role.value) });
        await onLoggedIn(result);
        navigator.navigate("HomeScreen");
    });

    const showErrorAlert = (errors: ApplicationError[]) => Alert.alert(
        'Register attempt was unsuccessful',
        errors[0].description,
        [{
            text: 'OK',
            onPress: () => clearErrors(),
            style: 'cancel',
        }]);

    if (errors.length > 0) {
        showErrorAlert(errors);
    }

    return (
        <TouchableWithoutFeedback onPress={Keyboard.dismiss} accessible={false}>
            <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : 'height'}>
                <Card style={styles.card}>
                    <Card.Title title="Register" />
                    <Card.Content>
                        <TextInput
                            label="First name"
                            value={firstName}
                            onChangeText={setFirstName}
                            style={styles.input}
                        />
                        <TextInput
                            label="Last name"
                            value={lastName}
                            onChangeText={setLastName}
                            style={styles.input}
                        />
                        <TextInput
                            label="Email"
                            value={email}
                            onChangeText={setEmail}
                            style={styles.input}
                        />
                        <TextInput
                            label="Password"
                            value={password}
                            onChangeText={setPassword}
                            secureTextEntry
                            style={styles.input}
                        />
                        <Menu
                            visible={visible}
                            onDismiss={closeMenu}
                            anchor={<Button onPress={openMenu}>{role ? role.label : "Select a role..."}</Button>}
                        >
                            <Menu.Item onPress={() => { setRole({ label: 'Student', value: '1' }); closeMenu(); }} title="Student" />
                            <Menu.Item onPress={() => { setRole({ label: 'Teacher', value: '2' }); closeMenu(); }} title="Teacher" />
                        </Menu>
                        <Button style={styles.registerButton} mode="contained" onPress={handleRegister}>
                            Register
                        </Button>
                    </Card.Content>
                </Card>
            </KeyboardAvoidingView>
        </TouchableWithoutFeedback>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        padding: 16,
    },
    card: {
        padding: 16,
    },
    input: {
        marginBottom: 16,
    },
    registerButton: {
        marginTop: 16,
    }
});

export default RegisterScreen;

