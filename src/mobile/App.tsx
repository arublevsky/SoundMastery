import React from 'react';
import {AppStack} from "./components/app-stack.tsx";
import AuthenticationProvider from "./modules/authorization/authenticationProvider.tsx";

function App(): React.JSX.Element {
  return (
      <AuthenticationProvider>
        <AppStack/>
      </AuthenticationProvider>
  );
}

export default App;
