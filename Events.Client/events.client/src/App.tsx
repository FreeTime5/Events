import { createContext, useContext, useEffect, useState } from "react";
import "./App.css";
import {
  ChakraProvider,
  Grid,
  GridItem,
  useStatStyles,
} from "@chakra-ui/react";
import { Pages, serverUrl, User } from "./constants";
import HeaderMenu from "./Menu/HeaderMenu";
import axios from "axios";
import { NavigationContainer } from "@react-navigation/native";
import { createNativeStackNavigator } from "@react-navigation/native-stack";

const userContext = createContext(null);

const Stack = createNativeStackNavigator();

function App() {
  const [pageNumber, setPageNumber] = useState(1);
  let data = "";
  useEffect(() => {
    axios.get(`${serverUrl}/Event/Events/${pageNumber}`).then((response) => {
      data = response.data;
    });
  }, []);

  const [user, setUser] = useState<User>({
    userName: "",
    role: "",
    isSignIn: false,
  });
  const [page, setPage] = useState(Pages.home);

  return (
    <ChakraProvider>
      <Grid templateColumns="20% 80%">
        <GridItem colSpan={2}>
          <HeaderMenu user={user} updatePage={setPage}></HeaderMenu>
        </GridItem>
        <NavigationContainer>
          <Stack.Navigator>
            <Stack.Screen name="Home" component={HomeScreen} />
          </Stack.Navigator>
        </NavigationContainer>
      </Grid>
    </ChakraProvider>
  );
}

export default App;
