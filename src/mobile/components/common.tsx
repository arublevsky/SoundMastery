import { Alert, StyleSheet } from "react-native";

export const showComfirmationAlert = (
    header: string,
    text: string,
    handler: () => Promise<void | null> | void
) => {
    Alert.alert(
        header,
        text,
        [
            { text: "Cancel", style: "cancel" },
            { text: "OK", onPress: handler }
        ],
        { cancelable: false }
    );
}

export const showSuccessAlert = (
    text: string,
    handler: () => Promise<void | null> | void
) => {
    Alert.alert(
        'Success',
        text,
        [{
            text: 'OK',
            onPress: handler,
            style: 'cancel',
        }]);
}

export const showErrorAlert = (text: string, handler?: () => Promise<void | null> | void ) => {
    Alert.alert(
        'Error',
        text,
        [{
            text: 'OK',
            onPress: handler,
            style: 'cancel',
        }]);
}

export const pickerSelectStyles = StyleSheet.create({
    inputIOS: {
        fontSize: 16,
        paddingVertical: 12,
        paddingHorizontal: 10,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 8,
        color: 'black',
        paddingRight: 30,
        marginBottom: 15,
        backgroundColor: '#fff',
    },
    inputAndroid: {
        fontSize: 16,
        paddingHorizontal: 10,
        paddingVertical: 8,
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius: 8,
        color: 'black',
        paddingRight: 30,
        marginBottom: 15,
        backgroundColor: '#fff',
    },
});