import React, {useState} from 'react';
import {Alert, StyleSheet, Text, TextInput, TouchableOpacity, View} from 'react-native';
import {useAuthContext} from "../modules/authorization/context.ts";
import {useErrorHandling} from "../modules/errors/useErrorHandling.tsx";
import {login} from "../modules/authorization/accountApi.ts";
import {ApplicationError} from "../modules/common/errorHandling.ts";
import {useNavigation} from "@react-navigation/native";
import {LoginScreenNavigationProps} from "./types.ts";

const LoginScreen = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const {onLoggedIn} = useAuthContext();
    const navigator = useNavigation<LoginScreenNavigationProps>();

    const handleLogin = () => asyncHandler(async () => {
        const result = await login(username, password);
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
        <View style={styles.container}>
            <Text style={styles.title}>Login</Text>

            <TextInput
                style={styles.input}
                placeholder="Username"
                value={username}
                onChangeText={(text) => setUsername(text)}
            />

            <TextInput
                style={styles.input}
                placeholder="Password"
                secureTextEntry={true}
                value={password}
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
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        marginBottom: 20,
    },
    input: {
        width: '80%',
        height: 40,
        borderColor: 'gray',
        borderWidth: 1,
        marginBottom: 10,
        paddingHorizontal: 10,
    },
    button: {
        backgroundColor: 'blue',
        padding: 10,
        borderRadius: 5,
    },
    buttonText: {
        color: 'white',
        fontSize: 16,
        fontWeight: 'bold',
    },
    orContainer: {
        marginVertical: 10,
        alignItems: 'center',
    },
    orText: {
        color: 'black',
        fontSize: 16,
        fontWeight: 'bold',
    },
});

export default LoginScreen;
