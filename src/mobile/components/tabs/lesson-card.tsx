import React from 'react';
import { Image, StyleSheet } from 'react-native';
import { Lesson } from '../../modules/api/lessonsApi';
import { Card, Paragraph } from 'react-native-paper';
import { useNavigation } from '@react-navigation/native';
import { HomeTabScreenProps } from '../types';
import { formatFullName } from '../utils';

interface LessonCardProps {
    lesson: Lesson;
    isTeacher: boolean;
    id: string;
};

const LessonCard = ({ lesson, isTeacher, id }: LessonCardProps) => {
    const navigation = useNavigation<HomeTabScreenProps<'MyLessons'>['navigation']>();

    const navigateToLessonDetails = () => {
        navigation.navigate('LessonDetailsScreen', { lesson, isTeacher });
    }

    return (
        <Card onPress={() => navigateToLessonDetails()} style={styles.card} id={id}>
            <Card.Title
                title={`${formatFullName(isTeacher ? lesson.teacher : lesson.student)}`}
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