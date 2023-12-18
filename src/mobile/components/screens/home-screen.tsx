import React from "react";
import {createMaterialBottomTabNavigator} from "react-native-paper/react-navigation";
import MyLessonsTab from "../tabs/my-lessons-tab.tsx";
import ScheduleLessonTab from "../tabs/schedule-lesson-tab.tsx";
import ProfileTab from "../tabs/profile-tab.tsx";

const Tab = createMaterialBottomTabNavigator();

function HomeScreen(): React.JSX.Element {
    return (
        <Tab.Navigator initialRouteName="My Lessons">
            <Tab.Screen
                name="MyLessons"
                component={MyLessonsTab}
                options={{
                    tabBarLabel: 'Home'
                }}
            />
            <Tab.Screen
                name="ScheduleLesson"
                component={ScheduleLessonTab}
                options={{
                    tabBarLabel: 'Schedule a Lesson',
                    tabBarBadge: 3,
                }}
            />
            <Tab.Screen
                name="Profile"
                component={ProfileTab}
                options={{
                    tabBarLabel: 'Profile'
                }}
            />
        </Tab.Navigator>
    );
}

export default HomeScreen;
