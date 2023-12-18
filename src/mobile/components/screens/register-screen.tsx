import React, {useState} from 'react';
import {
    Text,
    TextInput,
    TouchableOpacity,
    StyleSheet,
    Alert,
    Keyboard,
    TouchableWithoutFeedback, Platform, KeyboardAvoidingView
} from 'react-native';
import {useErrorHandling} from "../../modules/errors/useErrorHandling.tsx";
import {registerUser} from "../../modules/api/accountApi.ts";
import {useAuthContext} from "../../modules/authorization/context.ts";
import {ApplicationError} from "../../modules/common/errorHandling.ts";
import {useNavigation} from "@react-navigation/native";
import {RegisterScreenNavigationProps} from "../types.ts";
import RNPickerSelect from "react-native-picker-select";
import {pickerSelectStyles} from "../common.tsx";

const RegisterScreen = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [role, setRole] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const {onLoggedIn} = useAuthContext();
    const navigator = useNavigation<RegisterScreenNavigationProps>();

    const handleRegister = () => asyncHandler(async () => {
        if (!email || !firstName || !lastName || !password || !role) {
            Alert.alert('Error', 'Please fill all required fields');
            return;
        }

        const result = await registerUser({email, firstName, lastName, password, role: Number(role)});
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
                <Text style={styles.title}>Register</Text>
                <TextInput
                    style={styles.input}
                    placeholder="First Name"
                    placeholderTextColor='#242424'
                    value={firstName}
                    onChangeText={setFirstName}
                />
                <TextInput
                    style={styles.input}
                    placeholder="Last Name"
                    placeholderTextColor='#242424'
                    value={lastName}
                    onChangeText={setLastName}
                />
                <TextInput
                    style={styles.input}
                    placeholder="Email"
                    placeholderTextColor='#242424'
                    value={email}
                    onChangeText={setEmail}
                    keyboardType="email-address"
                />
                <TextInput
                    style={styles.input}
                    placeholder="Password"
                    placeholderTextColor='#242424'
                    value={password}
                    textContentType='oneTimeCode'
                    onChangeText={setPassword}
                    secureTextEntry={true}
                />
                <RNPickerSelect
                    items={[{text: "Teacher", value: "2"}, {text: "Student", value: "1"}]
                        .map(item => ({
                            label: item.text,
                            value: item.value
                        }))}
                    onValueChange={setRole}
                    placeholder={{label: 'Select your role', value: ''}}
                    value={role}
                    style={pickerSelectStyles}
                />
                <TouchableOpacity style={styles.button} onPress={handleRegister}>
                    <Text style={styles.buttonText}>Register</Text>
                </TouchableOpacity>
            </KeyboardAvoidingView>
        </TouchableWithoutFeedback>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#f5f5f5',
        padding: 16,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        marginBottom: 20,
        color: '#333',
    },
    input: {
        width: '100%',
        height: 40,
        borderColor: '#ccc',
        borderWidth: 1,
        marginBottom: 16,
        paddingHorizontal: 10,
        borderRadius: 8,
        backgroundColor: '#fff',
    },
    button: {
        backgroundColor: '#2196F3',
        padding: 15,
        borderRadius: 8,
        alignItems: 'center',
        marginBottom: 16,
    },
    buttonText: {
        color: '#fff',
        fontSize: 16,
        fontWeight: 'bold',
    },
});

export default RegisterScreen;

