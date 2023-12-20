import { NativeStackNavigationProp, NativeStackScreenProps } from "@react-navigation/native-stack";
import { CompositeScreenProps } from "@react-navigation/native";
import { MaterialBottomTabScreenProps } from "react-native-paper/react-navigation";
import { Lesson } from "../modules/api/lessonsApi";

export type RootStackParamList = {
    LoginScreen: undefined;
    HomeScreen: undefined;
    RegisterScreen: undefined;
};

export type BottomBarParamList = {
    MyLessons: { refreshToken: string };
    ScheduleLesson: undefined;
    Profile: undefined;
    LessonDetailsScreen: { lesson: Lesson, isTeacher: boolean };   
};

export type RegisterScreenNavigationProps = NativeStackNavigationProp<RootStackParamList, "RegisterScreen">;

export type LoginScreenNavigationProps = NativeStackNavigationProp<RootStackParamList, "LoginScreen">;

export type RootStackScreenProps<T extends keyof RootStackParamList> = NativeStackScreenProps<RootStackParamList, T>;

export type HomeTabScreenProps<T extends keyof BottomBarParamList> =
    CompositeScreenProps<
        MaterialBottomTabScreenProps<BottomBarParamList, T>,
        RootStackScreenProps<keyof RootStackParamList>
    >;