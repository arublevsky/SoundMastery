import React, { useState, useEffect, useMemo } from 'react';
import {
    View,
    Alert,
    StyleSheet,
    Keyboard,
    Platform,
    KeyboardAvoidingView,
    TouchableWithoutFeedback
} from 'react-native';
import RNPickerSelect from 'react-native-picker-select';
import { getTeachers, TeacherProfile } from "../../modules/api/teachersApi.ts";
import { addLesson, getAvailableLessons } from "../../modules/api/lessonsApi.ts";
import { useNavigation } from "@react-navigation/native";
import { ScreenProps } from "../types.ts";
import DateTimePicker, { DateTimePickerEvent } from '@react-native-community/datetimepicker';
import { useErrorHandling } from "../../modules/errors/useErrorHandling.tsx";
import { v4 as uuidv4 } from 'uuid';
import { pickerSelectStyles, showErrorAlert, showSuccessAlert } from "../common.tsx";
import { Button, Card, TextInput } from 'react-native-paper';

const ScheduleLesson = () => {
    const [teachers, setTeachers] = useState<TeacherProfile[]>([]);
    const [selectedTeacherId, setSelectedTeacherId] = useState('');
    const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    const [availableHours, setAvailableHours] = useState<number[]>([]);
    const [selectedHour, setSelectedHour] = useState('');
    const [description, setDescription] = useState('');
    const navigator = useNavigation<ScreenProps<'ScheduleLesson'>['navigation']>();
    const [errors, asyncHandler, clearErrors] = useErrorHandling();

    useEffect(() => {
        const fetchTeachers = async () => {
            const response = await getTeachers();
            setTeachers(response);
        };

        fetchTeachers();
    }, []);

    const getAvailableHours = useMemo(() => {
        if (!selectedTeacherId) {
            return { availableHours: [] };
        }
        return getAvailableLessons(selectedTeacherId, selectedDate);
    }, [selectedTeacherId, selectedDate]);

    const clearState = () => {
        setSelectedTeacherId('');
        setSelectedDate(new Date());
        setAvailableHours([]);
        setSelectedHour('');
        setDescription('');
    }

    const handleSchedule = () => asyncHandler(async () => {
        if (!selectedTeacherId || !selectedDate || !selectedHour) {
            Alert.alert('Error', 'Please fill all required fields');
            return;
        }

        await addLesson({
            teacherId: selectedTeacherId,
            description: description,
            date: selectedDate!,
            hour: Number(selectedHour!),
        });

        showSuccessAlert('Lesson scheduled successfully', () => {
            clearState();
            navigator.jumpTo("MyLessons", { refreshToken: uuidv4() })
        });
    });

    if (errors.length) {
        showErrorAlert(`Failed to schedule lesson: ${errors[0].description}.`, clearErrors);
    }

    const handleDateChange = async (_: DateTimePickerEvent, date?: Date) => {
        setSelectedDate(date || new Date());
        setSelectedHour('');
        await refreshAvailableHours();
    };

    const handleTeacherChange = (value: string) => {
        setSelectedTeacherId(value);
        setSelectedDate(new Date());
        setSelectedHour('');
    }

    const handleTeacherDonePress = () => {
        if (selectedTeacherId) {
            refreshAvailableHours();
        }
        else {
            setAvailableHours([]);
        }
    }

    const handleHourChange = (value: string) => {
        setSelectedHour(value);
    }

    const refreshAvailableHours = async () => {
        const response = await getAvailableHours;
        setAvailableHours(response?.availableHours || [])
    }

    return (
        <TouchableWithoutFeedback onPress={Keyboard.dismiss} accessible={false}>
            <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : 'height'}>
                <View style={styles.container}>
                    <Card style={styles.card}>
                        <Card.Title title="Schedule a Lesson" />
                        <Card.Content>
                            <RNPickerSelect
                                items={teachers.map(teacher => ({
                                    label: `${teacher.firstName} ${teacher.lastName}`,
                                    value: teacher.id.toString()
                                }))}
                                onValueChange={handleTeacherChange}
                                onDonePress={handleTeacherDonePress}
                                placeholder={{ label: 'Select a teacher', value: '' }}
                                value={selectedTeacherId}
                                style={pickerSelectStyles}
                            />
                            {selectedTeacherId
                                ? <>
                                    <View style={styles.datePickerContainer}>
                                        <DateTimePicker
                                            value={selectedDate}
                                            mode={'date'}
                                            is24Hour={true}
                                            display="default"
                                            minimumDate={new Date()}
                                            onChange={handleDateChange}
                                        />
                                    </View>
                                    {availableHours.length
                                        ? <RNPickerSelect
                                            items={availableHours.map(hour => ({
                                                label: `${hour}:00`,
                                                value: hour.toString()
                                            }))}
                                            onValueChange={handleHourChange}
                                            placeholder={{ label: 'Select time', value: '' }}
                                            value={selectedHour}
                                            style={pickerSelectStyles}
                                        />
                                        : null}
                                    {selectedHour
                                        ? <TextInput
                                            style={styles.input}
                                            placeholder="Enter description (optional)"
                                            value={description}
                                            onChangeText={(text) => setDescription(text)}
                                        />
                                        : null}
                                    {selectedHour
                                        ? <Button mode="contained" onPress={handleSchedule}>
                                            Schedule
                                        </Button>
                                        : null}
                                </>
                                : null}
                        </Card.Content>
                    </Card>
                </View>
            </KeyboardAvoidingView>
        </TouchableWithoutFeedback>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        padding: 16,
    },
    card: {
        padding: 16,
    },
    input: {
        marginBottom: 16,
    },
    datePickerContainer: {
        paddingBottom: 15,
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
    }
});

export default ScheduleLesson;
