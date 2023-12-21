import * as React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import HomeScreen from "./screens/home-screen.tsx";
import LoginScreen from "./screens/login-screen.tsx";
import { useAuthContext } from "../modules/authorization/context.ts";
import RegisterScreen from "./screens/register-screen.tsx";
import { Button } from "react-native";
import LessonDetailsScreen from './screens/lesson-details-screen.tsx';
import EditProfileScreen from './screens/edit-profile-screen.tsx';

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
                                title: 'Home',
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
                        <Stack.Screen<'EditProfileScreen'>
                            name="EditProfileScreen"
                            component={EditProfileScreen}
                            options={{
                                title: 'Edit Profile'
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