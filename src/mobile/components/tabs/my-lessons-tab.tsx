import React, { useEffect, useState } from "react";
import {
    Button,
    ScrollView,
    StyleSheet,
    Text,
    View
} from "react-native";
import { useAuthContext } from "../../modules/authorization/context.ts";
import { getMyLessons, Lesson } from "../../modules/api/lessonsApi.ts";
import { useNavigation, useRoute } from "@react-navigation/native";
import { HomeTabScreenProps } from "../types.ts";
import Icon from 'react-native-vector-icons/FontAwesome';

type MyLessonsProps = HomeTabScreenProps<'MyLessons'>;

function MyLessons(): React.JSX.Element {
    const { userProfile } = useAuthContext();
    const route = useRoute<MyLessonsProps['route']>();
    const navigation = useNavigation<MyLessonsProps['navigation']>();
    const [lessons, setLessons] = useState<Lesson[]>([]);

    useEffect(() => {
        const loadData = async () => {
            const lessonsData = await getMyLessons();
            setLessons(lessonsData);
        }

        loadData();
    }, [route]);

    const completedLessons = lessons.filter(l => l.completed);
    const upcomingLessons = lessons.filter(l => !l.completed);

    const renderLessonItem = (lesson: Lesson) => {
        const isTeacher = userProfile?.isTeacher || false;

        return (
            <View key={lesson.id} style={styles.card}>
                {!isTeacher ? <Text>Teacher: {lesson.teacher.firstName} {lesson.teacher.lastName}</Text> : null}
                {isTeacher ? <Text>Student: {lesson.student.firstName}  {lesson.student.lastName}</Text> : null}
                <Text>Start at: {`${new Date(lesson.date).toDateString()} at ${lesson.hour}:00`}</Text>
                {lesson.description ? <Text>Description: {`${lesson.description}`}</Text> : null}
                <Icon 
        name="chevron-right" 
        size={22}
        color="#777"
        style={styles.icon} 
      />
            </View>);
    }

    function renderLessonsBlock(lessons: Lesson[], title: string) {
        return lessons.length
            ? <>
                <Text style={styles.sectionTitle}>{title}</Text>
                {lessons.map((lesson) => renderLessonItem(lesson))}
            </>
            : null;
    }

    return (
        <ScrollView>
            <View style={styles.container}>
                <View style={styles.header}>
                    <Text style={styles.title}>My Lessons</Text>
                </View>
                <View style={styles.content}>
                    <View style={styles.emptyContainer}>
                        <Button
                            title="Schedule Your First Lesson"
                            onPress={() => navigation.navigate('ScheduleLesson')}
                        />
                    </View>
                    {upcomingLessons.length === 0
                        ? (<View style={styles.button}>
                            <Button
                                title="Schedule Your First Lesson"
                                color={styles.buttonText.color}
                                onPress={() => navigation.navigate('ScheduleLesson')}
                            />
                        </View>)
                        : <><Text>test</Text></>}
                    {renderLessonsBlock(upcomingLessons, "Upcoming Lessons")}
                    {renderLessonsBlock(completedLessons, "Completed Lessons")}
                </View>
            </View>
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff'
    },
    header: {
        backgroundColor: '#3f51b5',
        padding: 16,
    },
    title: {
        color: '#fff',
        fontSize: 18,
        fontWeight: '500'
    },
    content: {
        padding: 16
    },
    card: {
        backgroundColor: '#fff',
        borderRadius: 4,
        elevation: 2,
        shadowColor: '#000',
        shadowOpacity: 0.25,
        shadowOffset: { width: 0, height: 2 },
        shadowRadius: 8,
        marginBottom: 16,
        padding: 16,
        flexDirection: 'column',
        justifyContent: 'space-between',
        alignItems: 'center'
    },
    sectionTitle: {
        fontSize: 18,
        fontWeight: '500',
        marginTop: 16,
        marginBottom: 8
    },
    emptyContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        paddingHorizontal: 16
    },
    button: {
        backgroundColor: '#3f51b5',
        paddingVertical: 12,
        paddingHorizontal: 16,
        borderRadius: 4,
        elevation: 2,
        shadowColor: '#000',
        shadowOpacity: 0.25,
        shadowOffset: { width: 0, height: 2 },
        shadowRadius: 8,
    },
    buttonText: {
        color: '#fff',
        fontSize: 16,
        fontWeight: '500',
    },
    icon: {
        marginLeft: 8
      }
});

export default MyLessons;