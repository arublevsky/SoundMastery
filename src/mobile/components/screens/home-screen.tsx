import React from "react";
import {createMaterialBottomTabNavigator} from "react-native-paper/react-navigation";
import MyLessonsTab from "../tabs/my-lessons/my-lessons-tab.tsx";
import ScheduleLessonTab from "../tabs/schedule-lesson-tab.tsx";
import ProfileTab from "../tabs/profile-tab.tsx";
import Icon from "react-native-vector-icons/FontAwesome";

const Tab = createMaterialBottomTabNavigator();

function HomeScreen(): React.JSX.Element {
    return (
        <Tab.Navigator initialRouteName="My Lessons">
            <Tab.Screen
                name="MyLessons"
                component={MyLessonsTab}
                options={{
                    tabBarLabel: 'Home',
                    tabBarIcon: ({ color }) => (
                        <Icon name="home" color={color} size={24}  />
                    ),
                }}
            />
            <Tab.Screen
                name="ScheduleLesson"
                component={ScheduleLessonTab}
                options={{
                    tabBarLabel: 'Schedule a Lesson',
                    tabBarIcon: ({ color }) => (
                        <Icon name="calendar-plus-o" color={color} size={24}  />
                    ),
                }}
            />
            <Tab.Screen
                name="Profile"
                component={ProfileTab}
                options={{
                    tabBarLabel: 'Profile',
                    tabBarIcon: ({ color }) => (
                        <Icon name="user-circle-o" color={color} size={24} />
                    ),
                }}
            />
        </Tab.Navigator>
    );
}

export default HomeScreen;
