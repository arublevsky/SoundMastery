import React, {PropsWithChildren, useEffect, useState} from "react";
import {
    Animated,
    SafeAreaView,
    ScrollView,
    StatusBar,
    StyleSheet,
    Text,
    TouchableOpacity,
    useColorScheme,
    View
} from "react-native";
import {
    Colors,
    DebugInstructions,
    Header,
    LearnMoreLinks,
    ReloadInstructions
} from "react-native/Libraries/NewAppScreen";
import {useAuthContext} from "../modules/authorization/context.ts";
import {getMyTeachers, TeacherProfile} from "../modules/api/teachersApi.ts";
import {UserProfile} from "../modules/api/profileApi.ts";
import FlatList = Animated.FlatList;
import {ListRenderItem} from "@react-native/virtualized-lists";
import {ListRenderItemInfo} from "@react-native/virtualized-lists/Lists/VirtualizedList";
import {getMyLessons, Lesson} from "../modules/api/lessonsApi.ts";

type SectionProps = PropsWithChildren<{
    title: string;
}>;

function Section({children, title}: SectionProps): React.JSX.Element {
    const isDarkMode = useColorScheme() === 'dark';
    return (
        <View style={styles.sectionContainer}>
            <Text
                style={[
                    styles.sectionTitle,
                    {
                        color: isDarkMode ? Colors.white : Colors.black,
                    },
                ]}>
                {title}
            </Text>
            <Text
                style={[
                    styles.sectionDescription,
                    {
                        color: isDarkMode ? Colors.light : Colors.dark,
                    },
                ]}>
                {children}
            </Text>
        </View>
    );
}

function HomeScreen(): React.JSX.Element {
    const isDarkMode = useColorScheme() === 'dark';
    const {userProfile} = useAuthContext();
    const [teachers, setTeachers] = useState<TeacherProfile[]>([]);
    const [lessons, setLessons] = useState<Lesson[]>([]);

    const backgroundStyle = {
        backgroundColor: isDarkMode ? Colors.darker : Colors.lighter,
    };

    useEffect(() => {
        async function loadMyTeachers() {
            const teachers = await getMyTeachers();
            setTeachers(teachers);
        }

        async function loadMyLessons() {
            const lessons = await getMyLessons();
            setLessons(lessons);
        }

        loadMyLessons();
        loadMyTeachers();
    }, []);

    return (
        <SafeAreaView style={backgroundStyle}>
            <StatusBar
                barStyle={isDarkMode ? 'light-content' : 'dark-content'}
                backgroundColor={backgroundStyle.backgroundColor}
            />
            <ScrollView
                contentInsetAdjustmentBehavior="automatic"
                style={backgroundStyle}>
                <Header/>
                <View
                    style={{
                        backgroundColor: isDarkMode ? Colors.black : Colors.white,
                    }}>
                    <Section title="Welcome">
                        Hello <Text style={styles.highlight}>{userProfile!.firstName} {userProfile!.lastName}</Text>.
                    </Section>
                    <Section title="Here is your email:">
                        <Text style={styles.highlight}>{userProfile!.email}</Text>
                    </Section>
                    <Section title="Here is the teachers list:">
                        <ScrollView>
                            {teachers.map((teacher) => (
                                <View key={teacher.id} style={styles.teacherItem}>
                                    <Text>{`${teacher.firstName} ${teacher.lastName}`}</Text>
                                </View>
                            ))}
                        </ScrollView>
                    </Section>
                    <Section title="Here is the lessons list:">
                        <ScrollView>
                            {lessons.map((lesson) => (
                                <View key={lesson.id} style={styles.teacherItem}>
                                    <Text>{lesson.teacherFullname}</Text>
                                    <Text>Completed: {`${lesson.completed}`}</Text>
                                    <Text>Start at: {`${lesson.startAt}`}</Text>
                                </View>
                            ))}
                        </ScrollView>
                    </Section>
                </View>
            </ScrollView>
        </SafeAreaView>
    );
}

const styles = StyleSheet.create({
    sectionContainer: {
        marginTop: 32,
        paddingHorizontal: 24,
    },
    sectionTitle: {
        fontSize: 24,
        fontWeight: '600',
    },
    sectionDescription: {
        marginTop: 8,
        fontSize: 18,
        fontWeight: '400',
    },
    highlight: {
        fontWeight: '700',
    },
    container: {
        flex: 1,
        justifyContent: 'flex-start',
        alignItems: 'flex-start',
        padding: 16,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        marginBottom: 20,
    },
    teacherItem: {
        padding: 10,
        borderBottomWidth: 1,
        borderBottomColor: 'gray',
    },
});

export default HomeScreen;
