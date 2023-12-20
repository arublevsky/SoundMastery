import React, { useEffect, useState } from "react";
import {
    ScrollView,
    StyleSheet
} from "react-native";
import { useAuthContext } from "../../../modules/authorization/context.ts";
import { getMyLessons, Lesson } from "../../../modules/api/lessonsApi.ts";
import { useNavigation, useRoute } from "@react-navigation/native";
import { HomeTabScreenProps } from "../../types.ts";
import LessonCard from "./my-lesson-card.tsx";
import { Button, Card } from "react-native-paper";

type MyLessonsProps = HomeTabScreenProps<'MyLessons'>;

function MyLessons(): React.JSX.Element {
    const { userProfile } = useAuthContext();
    const route = useRoute<MyLessonsProps['route']>();
    const navigation = useNavigation<MyLessonsProps['navigation']>();
    const [lessons, setLessons] = useState<Lesson[]>([]);
    const isTeacher = userProfile!.isTeacher;

    useEffect(() => {
        const loadData = async () => {
            const lessonsData = await getMyLessons();
            setLessons(lessonsData);
        }

        loadData();
    }, [route]);

    const completedLessons = lessons.filter(l => l.completed || l.cancelled);
    const upcomingLessons = lessons.filter(l => !l.completed && !l.cancelled);

    const renderLessonItem = (lesson: Lesson) => {
        
        return (<LessonCard lesson={lesson} isTeacher={isTeacher} key={lesson.id} />);
    }

    const renderLessonsBlock = (lessons: Lesson[], title: string) => {
        return lessons.length
            ? <Card style={styles.card} key={title}>
                <Card.Title title={title} />
                <Card.Content>
                    {lessons.map((lesson) => renderLessonItem(lesson))}
                </Card.Content>
            </Card>
            : null;
    }

    return (
        <ScrollView style={styles.container}>
            {upcomingLessons.length === 0
                ? <Card style={styles.card} key="schedule-next">
                    <Card.Title title="You have no upcoming lessons" />
                    <Card.Content>
                        <Button mode="contained" onPress={() => navigation.navigate('ScheduleLesson')}>
                            Schedule Your Next Lesson
                        </Button>
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
    }
});

export default MyLessons;