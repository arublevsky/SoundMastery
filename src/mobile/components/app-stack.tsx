import * as React from 'react';
import {NavigationContainer} from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import HomeScreen from "./home-screen.tsx";
import LoginScreen from "./login-screen.tsx";
import {useAuthContext} from "../modules/authorization/context.ts";
import RegisterScreen from "./register-screen.tsx";

const Stack = createNativeStackNavigator();

export const AppStack = () => {
    const {isAuthenticated} = useAuthContext();

    return (
        <NavigationContainer>
            <Stack.Navigator>
                {isAuthenticated && <Stack.Screen
                    name="HomeScreen"
                    component={HomeScreen}
                    options={{title: 'Hello world'}}
                />}
                {!isAuthenticated &&
                    <>
                        <Stack.Screen
                            name="LoginScreen"
                            component={LoginScreen}
                            options={{title: 'Login screen'}}
                        />
                        <Stack.Screen
                            name="RegisterScreen"
                            component={RegisterScreen}
                            options={{title: 'Register screen'}}
                        />
                    </>}
            </Stack.Navigator>
        </NavigationContainer>
    );
}