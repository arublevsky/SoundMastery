import React, {useState, useEffect, useMemo} from 'react';
import 'react-native-get-random-values';
import {
    View,
    Text,
    TouchableOpacity,
    Alert,
    StyleSheet,
    TextInput,
    Keyboard,
    Platform,
    KeyboardAvoidingView, TouchableWithoutFeedback
} from 'react-native';
import RNPickerSelect from 'react-native-picker-select';
import {getTeachers, TeacherProfile} from "../../modules/api/teachersApi.ts";
import {addLesson, getAvailableLessons} from "../../modules/api/lessonsApi.ts";
import {useNavigation} from "@react-navigation/native";
import {HomeTabScreenProps} from "../types.ts";
import DateTimePicker, {DateTimePickerEvent} from '@react-native-community/datetimepicker';
import {useErrorHandling} from "../../modules/errors/useErrorHandling.tsx";
import {v4 as uuidv4} from 'uuid';
import {pickerSelectStyles} from "../common.tsx";

const ScheduleLesson = () => {
    const [teachers, setTeachers] = useState<TeacherProfile[]>([]);
    const [selectedTeacherId, setSelectedTeacherId] = useState('');
    const [selectedDate, setSelectedDate] = useState<Date>(new Date());
    const [availableHours, setAvailableHours] = useState<number[]>([]);
    const [selectedHour, setSelectedHour] = useState('');
    const [description, setDescription] = useState('');
    const navigator = useNavigation<HomeTabScreenProps<'ScheduleLesson'>['navigation']>();
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
            return {availableHours: []};
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

        Alert.alert(
            'Success',
            'Lesson scheduled successfully',
            [{
                text: 'OK',
                onPress: () => {
                    clearState();
                    navigator.jumpTo("MyLessons", {refreshToken: uuidv4()})
                },
                style: 'cancel',
            }]);
    });

    if (errors.length) {
        Alert.alert(
            'Error',
            `Failed to schedule lesson: ${errors[0].description}.`,
            [{
                text: 'OK',
                onPress: () => clearErrors(),
                style: 'cancel',
            }]);
    }

    const handleDateChange = async (_: DateTimePickerEvent, date?: Date) => {
        setSelectedDate(date || new Date());
        setSelectedHour('');
        await refreshAvailableHours();
    };

    const handleTeacherChange = async (value: string) => {
        if (value) {
            await refreshAvailableHours();
        }

        if (value != selectedTeacherId) {
            setAvailableHours([]);
        }

        setSelectedTeacherId(value);
    }

    const handleHourChange = async (value: string) => {
        setSelectedHour(value);
    }

    const refreshAvailableHours = async () => {
        const response = await getAvailableHours;
        setAvailableHours(response?.availableHours || [])
    }

    return (
        <TouchableWithoutFeedback onPress={Keyboard.dismiss} accessible={false}>
            <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : 'height'}>
                <Text style={styles.title}>Schedule Lesson</Text>

                <RNPickerSelect
                    items={teachers.map(teacher => ({
                        label: `${teacher.firstName} ${teacher.lastName}`,
                        value: teacher.id.toString()
                    }))}
                    onValueChange={handleTeacherChange}
                    placeholder={{label: 'Select a teacher', value: ''}}
                    value={selectedTeacherId}
                    style={pickerSelectStyles}
                />
                {selectedTeacherId
                    ? <View style={styles.datePickerContainer}>
                        <DateTimePicker
                            value={selectedDate}
                            mode={'date'}
                            is24Hour={true}
                            display="default"
                            minimumDate={new Date()}
                            onChange={handleDateChange}
                        />
                    </View>
                    : null}
                {availableHours.length
                    ? <RNPickerSelect
                        items={availableHours.map(hour => ({
                            label: `${hour}:00`,
                            value: hour.toString()
                        }))}
                        onValueChange={handleHourChange}
                        placeholder={{label: 'Select time', value: ''}}
                        value={selectedHour}
                        style={pickerSelectStyles}
                    />
                    : <Text>No free slots available</Text>}

                {selectedHour
                    ? <TextInput
                        style={styles.input}
                        placeholder="Enter description (optional)"
                        value={description}
                        onChangeText={(text) => setDescription(text)}
                    />
                    : null}
                {selectedHour
                    ? <TouchableOpacity style={styles.button} onPress={handleSchedule}>
                        <Text style={styles.buttonText}>Schedule</Text>
                    </TouchableOpacity>
                    : null}
            </KeyboardAvoidingView>
        </TouchableWithoutFeedback>
    );
};

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#f5f5f5',
        padding: 16,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        marginBottom: 20,
    },
    inputContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        marginBottom: 20,
        backgroundColor: '#fff',
        padding: 10,
        borderRadius: 8,
    },
    icon: {
        marginLeft: 'auto',
    },
    input: {
        width: '100%',
        height: 40,
        borderColor: '#ccc',
        borderWidth: 1,
        marginBottom: 20,
        paddingHorizontal: 10,
        borderRadius: 8,
        backgroundColor: '#fff',
    },
    button: {
        backgroundColor: '#2196F3',
        padding: 15,
        borderRadius: 8,
        alignItems: 'center',
    },
    buttonText: {
        color: '#fff',
        fontSize: 16,
        fontWeight: 'bold',
    },
    datePickerContainer: {
        padding: 15,
    }
});

export default ScheduleLesson;
