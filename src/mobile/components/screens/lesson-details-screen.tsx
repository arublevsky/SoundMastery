import React from 'react';
import { StyleSheet, View } from 'react-native';
import { HomeTabScreenProps } from '../types';
import { Button, Card } from 'react-native-paper';
import { cancel, complete } from '../../modules/api/lessonsApi';
import { useErrorHandling } from '../../modules/errors/useErrorHandling';
import { showComfirmationAlert, showErrorAlert, showSuccessAlert } from '../common';
import { v4 as uuidv4 } from 'uuid';
import LessonCardContent from '../tabs/my-lessons/my-lesson-card-content';

type Props = HomeTabScreenProps<'LessonDetailsScreen'>;

const LessonDetailsScreen = ({ route, navigation }: Props) => {
    const { lesson, isTeacher } = route.params;
    const [errors, asyncHandler, clearErrors] = useErrorHandling();

    const handleCancel = () => showComfirmationAlert(
        "Cancel Lesson",
        "Are you sure you want to cancel this lesson?",
        cancelLesson);

    const handleComplete = () => showComfirmationAlert(
        "Complete Lesson",
        "Are you sure you want to complete this lesson?",
        completeLesson);


    const cancelLesson = () => asyncHandler(async () => {
        await cancel(lesson.id);
        onUpdateSuccess('Lesson cancelled successfully');
    });

    const completeLesson = () => asyncHandler(async () => {
        await complete(lesson.id);
        onUpdateSuccess('Lesson completed successfully');
    });

    if (errors.length) {
        showErrorAlert(`Failed to update a lesson: ${errors[0].description}.`, clearErrors);
    }

    const onUpdateSuccess = (message: string) => {
        showSuccessAlert(message, () => {
            clearErrors();
            navigation.navigate("MyLessons", { refreshToken: uuidv4() })
        });
    };

    const editable = !lesson.cancelled && !lesson.completed;

    return (
        <View style={styles.container}>
            <Card style={styles.card}>
                <LessonCardContent lesson={lesson} isTeacher={isTeacher} />
            </Card>
            {editable
                ? <View style={styles.buttonContainer}>
                    <Button mode="contained" onPress={handleComplete} style={styles.button} buttonColor='#3f50b5'>
                        Complete Lesson
                    </Button>
                    <Button mode="contained" onPress={handleCancel} style={styles.button} buttonColor='#f44336'>
                        Cancel Lesson
                    </Button>
                </View>
                : null}

        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'space-between',
    },
    button: {
        margin: 8,
    },
    buttonContainer: {
        marginBottom: 16,
    },
    card: {
        margin: 10,
    }
});

export default LessonDetailsScreen;
