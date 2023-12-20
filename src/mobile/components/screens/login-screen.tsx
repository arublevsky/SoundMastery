import React, {useState} from 'react';
import {
    Alert,
    Keyboard, KeyboardAvoidingView, Platform,
    StyleSheet,
    Text,
    TouchableWithoutFeedback,
    View
} from 'react-native';
import {useNavigation} from "@react-navigation/native";
import {useErrorHandling} from "../../modules/errors/useErrorHandling.tsx";
import {LoginScreenNavigationProps} from "../types.ts";
import {useAuthContext} from "../../modules/authorization/context.ts";
import {login} from "../../modules/api/accountApi.ts";
import {ApplicationError} from "../../modules/common/errorHandling.ts";
import { Button, Card, TextInput } from 'react-native-paper';

const LoginScreen = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, asyncHandler, clearErrors] = useErrorHandling();
    const {onLoggedIn} = useAuthContext();
    const navigator = useNavigation<LoginScreenNavigationProps>();

    const handleLogin = () => asyncHandler(async () => {
        const result = await login("Alex.jan@g.com", "UserPass123!");
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
            <Card style={styles.card}>
          <Card.Title title="Login" />
          <Card.Content>
            <TextInput
              label="Username"
              value={username}
              onChangeText={setUsername}
              style={styles.input}
            />
            <TextInput
              label="Password"
              value={password}
              onChangeText={setPassword}
              secureTextEntry
              style={styles.input}
            />
            <Button mode="contained" onPress={handleLogin}>
              Login
            </Button>
            <View style={styles.orContainer}>
              <Text style={styles.orText}>OR</Text>
            </View>
            <Button mode="contained" onPress={handleRegister}>
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
    orContainer: {
      alignItems: 'center',
      justifyContent: 'center',
      marginVertical: 16,
    },
    orText: {
      fontSize: 16,
      color: '#888',
    },
  });

export default LoginScreen;