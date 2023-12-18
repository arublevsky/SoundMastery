import React, {PropsWithChildren} from "react";
import {
    SafeAreaView,
    ScrollView,
    StatusBar,
    StyleSheet,
    Text,
    View
} from "react-native";
import {
    Colors
} from "react-native/Libraries/NewAppScreen";
import {useAuthContext} from "../../modules/authorization/context.ts";

type SectionProps = PropsWithChildren<{ title: string; }>;

function Section({children, title}: SectionProps): React.JSX.Element {
    return (
        <View style={styles.sectionContainer}>
            <Text style={[styles.sectionTitle, {color: Colors.black}]}>
                {title}
            </Text>
            <Text style={[styles.sectionDescription, {color: Colors.dark}]}>
                {children}
            </Text>
        </View>
    );
}

const backgroundStyle = {
    backgroundColor: Colors.lighter,
};

function ProfileTab(): React.JSX.Element {
    const {userProfile} = useAuthContext();

    return (
        <SafeAreaView style={backgroundStyle}>
            <StatusBar barStyle={'dark-content'} backgroundColor={backgroundStyle.backgroundColor}
            />
            <ScrollView contentInsetAdjustmentBehavior="automatic" style={backgroundStyle}>
                <View style={{backgroundColor: Colors.white}}>
                    <Section title={`Welcome, ${userProfile!.user.firstName} ${userProfile!.user.lastName}`}/>
                    <Section title={`Email: ${userProfile?.user.email}`} />
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
    lessonItem: {
        padding: 10,
        borderBottomWidth: 1,
        borderBottomColor: 'gray',
    },
});

export default ProfileTab;