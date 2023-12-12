import {NativeStackNavigationProp} from "@react-navigation/native-stack";

export type RootStackParamList = {
    LoginScreen: undefined;
    HomeScreen: undefined;
    RegisterScreen: undefined;
};

export type RegisterScreenNavigationProps = NativeStackNavigationProp<RootStackParamList, "RegisterScreen">;
export type LoginScreenNavigationProps = NativeStackNavigationProp<RootStackParamList, "LoginScreen">;