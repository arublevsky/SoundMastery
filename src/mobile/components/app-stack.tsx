import * as React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { NativeStackScreenProps, createNativeStackNavigator } from '@react-navigation/native-stack';
import HomeScreen from "./screens/home-screen.tsx";
import LoginScreen from "./screens/login-screen.tsx";
import { useAuthContext } from "../modules/authorization/context.ts";
import RegisterScreen from "./screens/register-screen.tsx";
import { Button } from "react-native";
import LessonDetailsScreen from './screens/lesson-details-screen.tsx';
import { RootStackParamList } from './types.ts';

const Stack = createNativeStackNavigator();

export const AppStack = () => {
    const { isAuthenticated, onLoggedOut } = useAuthContext();

    return (
        <NavigationContainer>
            <Stack.Navigator>
                {isAuthenticated
                    ?
                    <>
                        <Stack.Screen
                            name="HomeScreen"
                            component={HomeScreen}
                            options={{
                                title: 'SoundMastery',
                                headerRight: () => (
                                    <Button onPress={onLoggedOut} title="Logout" />
                                )
                            }}
                        />
                        <Stack.Screen<'LessonDetailsScreen'>
                            name="LessonDetailsScreen"
                            // @ts-ignore
                            component={LessonDetailsScreen}
                            options={{
                                title: 'Lesson Details'
                            }}
                        />
                    </>
                    : null}
                {!isAuthenticated
                    ? <>
                        <Stack.Screen
                            name="LoginScreen"
                            component={LoginScreen}
                            options={{ title: 'Login screen' }}
                        />
                        <Stack.Screen
                            name="RegisterScreen"
                            component={RegisterScreen}
                            options={{ title: 'Register screen' }}
                        />
                    </>
                    : null}
            </Stack.Navigator>
        </NavigationContainer>
    );
}