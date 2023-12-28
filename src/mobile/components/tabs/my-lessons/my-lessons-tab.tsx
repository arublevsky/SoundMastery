import React, { useCallback, useEffect, useState } from "react";
import {
    RefreshControl,
    ScrollView,
    StyleSheet
} from "react-native";
import { useAuthContext } from "../../../modules/authorization/context.ts";
import { getMyLessons, Lesson } from "../../../modules/api/lessonsApi.ts";
import { useNavigation, useRoute } from "@react-navigation/native";
import { ScreenProps } from "../../types.ts";
import { Button, Card } from "react-native-paper";
import LessonCardContent from "./my-lesson-card-content.tsx";

type MyLessonsProps = ScreenProps<'MyLessons'>;

function MyLessons(): React.JSX.Element {
    const { userProfile } = useAuthContext();
    const route = useRoute<MyLessonsProps['route']>();
    const navigation = useNavigation<MyLessonsProps['navigation']>();
    const [lessons, setLessons] = useState<Lesson[]>([]);
    const [refreshing, setRefreshing] = React.useState(false);

    const isTeacher = userProfile!.isTeacher;

    const loadData = async () => {
        setRefreshing(true);
        const lessonsData = await getMyLessons();
        setLessons(lessonsData);
        setRefreshing(false);
    }

    useEffect(() => {
        loadData();
    }, [route]);

    const completedLessons = lessons.filter(l => l.completed || l.cancelled);
    const upcomingLessons = lessons.filter(l => !l.completed && !l.cancelled);

    const onRefresh = useCallback(loadData, []);

    const renderScheduleNext = () => {
        return !isTeacher
            ? <Button mode="contained" onPress={() => navigation.navigate('ScheduleLesson')}>
                Schedule Your Next Lesson
            </Button>
            : <Button icon="check-all" pointerEvents='none'>
                All done 
            </Button>
    }

    const renderLessonsBlock = (lessons: Lesson[], title: string, scheduleNext: boolean = false) => {
        return !refreshing
            ? <Card style={styles.card} key={title}>
                <Card.Title title={title} style={styles.cardTitle}/>
                <Card.Content>
                    {lessons.length
                        ? lessons.map((lesson) =>
                            <Card
                                onPress={() => navigation.navigate('LessonDetailsScreen', { lesson, isTeacher })}
                                style={styles.card}
                                key={lesson.id.toString()}
                            >
                                <LessonCardContent lesson={lesson} isTeacher={isTeacher} />
                            </Card>)
                        : scheduleNext ? renderScheduleNext() : null}
                </Card.Content>
            </Card>
            : null;
    }

    return (
        <ScrollView style={styles.container}>
            <RefreshControl refreshing={refreshing} onRefresh={onRefresh} title="Loading..." />
            {renderLessonsBlock(upcomingLessons, "Upcoming lessons", true)}
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
        alignItems: 'center',
        
    },
    cardTitle: {
        width: '100%',
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