import React from 'react';
import { Image, StyleSheet } from 'react-native';
import { RootStackParamList } from '../types';
import { formatFullName } from '../utils';
import { Card, Paragraph } from 'react-native-paper';
import { NativeStackScreenProps } from '@react-navigation/native-stack';

type Props = NativeStackScreenProps<RootStackParamList, 'LessonDetailsScreen'>;

const LessonDetailsScreen = ({ route }: Props) => {
    const { lesson, isTeacher } = route.params;

    return (
        <Card style={styles.card}>
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

export default LessonDetailsScreen;