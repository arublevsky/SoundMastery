import * as React from 'react';
import {CompositeNavigationProp, NavigationContainer} from '@react-navigation/native';
import {createNativeStackNavigator, NativeStackNavigationProp} from '@react-navigation/native-stack';
import HelloWorld from "./hello-world.tsx";
import LoginScreen from "./login-screen.tsx";
import {useAuthContext} from "../modules/authorization/context.ts";

type RootStackParamList = {
    LoginScreen: undefined;
    HomeScreen: undefined;
};

export type RootNavigationProp = CompositeNavigationProp<
    NativeStackNavigationProp<RootStackParamList, 'LoginScreen'>,
    NativeStackNavigationProp<RootStackParamList, 'HomeScreen'>
>;

const Stack = createNativeStackNavigator();

export const AppStack = () => {
    const {isAuthenticated} = useAuthContext();

    return (
        <NavigationContainer>
            <Stack.Navigator>
                {isAuthenticated && <Stack.Screen
                    name="HomeScreen"
                    component={HelloWorld}
                    options={{title: 'Hello world'}}
                />}
                {!isAuthenticated && <Stack.Screen
                    name="LoginScreen"
                    component={LoginScreen}
                    options={{title: 'Login screen'}}
                />}
            </Stack.Navigator>
        </NavigationContainer>
    );
};