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
import LessonCard from "./lesson-card.tsx";
import { Card } from "react-native-paper";

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
        return (<LessonCard lesson={lesson} isTeacher={isTeacher} />);
    }

    const renderLessonsBlock = (lessons: Lesson[], title: string) => {
        return lessons.length
            ? <Card style={styles.card}>
                <Card.Title title={title} />
                <Card.Content>
                    {lessons.map((lesson) => renderLessonItem(lesson))}
                </Card.Content>
            </Card>
            : null;
    }

    return (
        // <ScrollView style={styles.container}>
        <ScrollView style={styles.container}>
            {upcomingLessons.length === 0
                ? <Card style={styles.card}>
                    <Card.Content>
                        <View style={styles.button}>
                            <Button
                                title="Schedule Your Next Lesson"
                                color={styles.buttonText.color}
                                onPress={() => navigation.navigate('ScheduleLesson')}
                            />
                        </View>
                    </Card.Content>
                </Card>
                : null}
            {renderLessonsBlock(upcomingLessons, "Upcoming lessons")}
            {renderLessonsBlock(completedLessons, "Completed Lessons")}
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