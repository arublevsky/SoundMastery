import React from 'react';
import { Image, StyleSheet } from 'react-native';
import { Lesson } from '../../modules/api/lessonsApi';
import { User } from '../../modules/api/profileApi';
import { Card, Paragraph } from 'react-native-paper';

interface LessonCardProps {
    lesson: Lesson;
    isTeacher: boolean;
};

const LessonCard = ({ lesson, isTeacher }: LessonCardProps) => {
    const navigateToLessonDetails = (lessonId: number) => {
        console.log(lessonId);
    }

    const formatFullName = (user: User) => `${user.firstName} ${user.lastName}`;
    
    return (
        <Card onPress={() => navigateToLessonDetails(lesson.id)} style={styles.card} id={lesson.id.toString()}>
        <Card.Title
            title={isTeacher ? `${formatFullName(lesson.student)}` : `${formatFullName(lesson.teacher)}`}
            left={() => <Image source={{ uri: 'https://w7.pngwing.com/pngs/340/946/png-transparent-avatar-user-computer-icons-software-developer-avatar-child-face-heroes-thumbnail.png' }} style={styles.avatar} />}
        />
        <Card.Content>
            <Paragraph>Start at: {`${new Date(lesson.date).toDateString()} at ${lesson.hour}:00`}</Paragraph>
            {lesson.description ? <Paragraph>{`${lesson.description}`}</Paragraph> : null}
        </Card.Content>
    </Card>
    );
};


const styles = StyleSheet.create({
    card: {
        margin: 10,
    },
    avatar: {
        width: 50,
        height: 50,
        borderRadius: 25,
    },
});

export default LessonCard;