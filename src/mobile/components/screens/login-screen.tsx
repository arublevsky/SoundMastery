import React, {useState} from 'react';
import {
    Alert,
    Keyboard, KeyboardAvoidingView, Platform,
    StyleSheet,
    Text,
    TextInput,
    TouchableOpacity,
    TouchableWithoutFeedback,
    View
} from 'react-native';
import {useNavigation} from "@react-navigation/native";
import {useErrorHandling} from "../../modules/errors/useErrorHandling.tsx";
import {LoginScreenNavigationProps} from "../types.ts";
import {useAuthContext} from "../../modules/authorization/context.ts";
import {login} from "../../modules/api/accountApi.ts";
import {ApplicationError} from "../../modules/common/errorHandling.ts";

const LoginScreen = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const {onLoggedIn} = useAuthContext();
    const navigator = useNavigation<LoginScreenNavigationProps>();

    const handleLogin = () => asyncHandler(async () => {
        const result = await login("test@test.com", "UserPass123!");
        await onLoggedIn(result);
    });

    const handleRegister = () => {
        navigator.navigate("RegisterScreen");
    }

    const showErrorAlert = (errors: ApplicationError[]) => Alert.alert(
        'Login attempt was unsuccessful',
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
                <Text style={styles.title}>Login</Text>
                <TextInput
                    style={styles.input}
                    placeholder="User name"
                    placeholderTextColor='#242424'
                    value={username}
                    onChangeText={(text) => setUsername(text)}
                />
                <TextInput
                    style={styles.input}
                    placeholder="Password"
                    placeholderTextColor='#242424'
                    secureTextEntry={true}
                    value={password}
                    textContentType='oneTimeCode'
                    onChangeText={(text) => setPassword(text)}
                />
                <TouchableOpacity style={styles.button} onPress={handleLogin}>
                    <Text style={styles.buttonText}>Login</Text>
                </TouchableOpacity>
                <View style={styles.orContainer}>
                    <Text style={styles.orText}>OR</Text>
                </View>
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
    orContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        marginBottom: 16,
    },
    orText: {
        marginHorizontal: 8,
        color: '#333',
    },
});

export default LoginScreen;
