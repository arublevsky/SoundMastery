import React from 'react';
import 'react-native-get-random-values';
import { AppStack } from "./components/app-stack.tsx";
import AuthenticationProvider from "./modules/authorization/authenticationProvider.tsx";
import { DefaultTheme, PaperProvider } from 'react-native-paper';

function App(): React.JSX.Element {
  return (
    <AuthenticationProvider>
      <PaperProvider theme={DefaultTheme}>
        <AppStack />
      </PaperProvider>
    </AuthenticationProvider>
  );
}

export default App;
