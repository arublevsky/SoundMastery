import React from 'react';
import { Lesson } from '../../../modules/api/lessonsApi';
import { Button, Card, Paragraph } from 'react-native-paper';
import { Image, StyleSheet } from 'react-native';
import { formatFullName } from '../../utils';
import { images } from '../../../assets';

interface Props {
    lesson: Lesson;
    isTeacher: boolean;
};

const LessonCardContent = ({ lesson, isTeacher }: Props) => {
    const active = !lesson.cancelled && !lesson.completed;
    const avatar = isTeacher ? lesson.student.avatar : lesson.teacher.avatar;
    const user = isTeacher ? lesson.student : lesson.teacher;
    const fullName = formatFullName(user);

    return (
        <>
            <Card.Title
                title={isTeacher ? `Student: ${fullName}` : `Teacher: ${fullName}`}
                left={() => <Image
                    source={avatar ? { uri: `data:image/png;base64,${avatar}` } : images.avatar}
                    style={styles.avatar}
                />}
            />
            <Card.Content>
                <Paragraph>Start at: {`${new Date(lesson.date).toDateString()} at ${lesson.hour}:00`}</Paragraph>
                {lesson.description ? <Paragraph>{`${lesson.description}`}</Paragraph> : null}
                {lesson.completed
                    ? <Button icon="check-all" pointerEvents='none'>
                        Completed
                    </Button>
                    : null}
                {lesson.cancelled
                    ? <Button icon="book-cancel-outline" pointerEvents='none'>
                        Cancelled
                    </Button>
                    : null}
                {active
                    ? <Button icon="clock-time-seven-outline" pointerEvents='none'>
                        Scheduled
                    </Button>
                    : null}
            </Card.Content>
        </>
    );
};

const styles = StyleSheet.create({
    avatar: {
        width: 50,
        height: 50,
        borderRadius: 25,
    }
});
export default LessonCardContent;