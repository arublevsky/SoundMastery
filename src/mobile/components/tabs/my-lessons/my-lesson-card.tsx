import React from 'react';
import { Image, StyleSheet, View } from 'react-native';
import { Lesson } from '../../../modules/api/lessonsApi';
import { Button, Card, Paragraph, Text } from 'react-native-paper';
import { useNavigation } from '@react-navigation/native';
import { HomeTabScreenProps } from '../../types';
import { formatFullName } from '../../utils';
import LessonCardContent from './my-lesson-card-content';

interface LessonCardProps {
    lesson: Lesson;
    isTeacher: boolean;
};

const LessonCard = ({ lesson, isTeacher }: LessonCardProps) => {
    const navigation = useNavigation<HomeTabScreenProps<'MyLessons'>['navigation']>();

    const navigateToLessonDetails = () => {
        navigation.navigate('LessonDetailsScreen', { lesson, isTeacher });
    }

    return (
        <Card onPress={() => navigateToLessonDetails()} style={styles.card} key={lesson.id.toString()}>
            <LessonCardContent lesson={lesson} isTeacher />
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
    }
});

export default LessonCard;