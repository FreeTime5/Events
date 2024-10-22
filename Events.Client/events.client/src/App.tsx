import { createContext, useState } from "react";
import "./App.css";
import { ChakraProvider, Grid, GridItem } from "@chakra-ui/react";
import { User } from "./constants";
import HeaderMenu from "./Menu/HeaderMenu";
import RegisterScreen from "./screens/RegisterScreen";

function App() {
  const [user, setUser] = useState<User>({
    jwtToken: "",
    refreshToken: "",
    isSignin: false,
  });

  return (
    <ChakraProvider>
      <Grid templateColumns="20% 80%">
        <GridItem colSpan={2}>
          <HeaderMenu user={user} navigation={user}></HeaderMenu>
        </GridItem>
        <GridItem colSpan={1}></GridItem>
        <GridItem colSpan={1}>
          <RegisterScreen user={user}></RegisterScreen>
        </GridItem>
      </Grid>
    </ChakraProvider>
  );
}

export default App;
