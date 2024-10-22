import { createContext, useCallback, useMemo, useState } from "react";
import "./App.css";
import { Button, ChakraProvider, Grid, GridItem } from "@chakra-ui/react";
import { Pages, User } from "./constants";
import HeaderMenu from "./Menu/HeaderMenu";
import RegisterScreen from "./screens/RegisterScreen";
import LoginScreen from "./screens/LoginScreen";
import HomeScreen from "./screens/HomeScreen";

function App() {
  const [page, setPage] = useState<Pages>(Pages.home);
  const [user, setUser] = useState<User>({
    jwtToken: "",
    refreshToken: "",
    isLogedIn: false,
  });

  const updateUser = useCallback(
    (user2: User) => {
      setUser(user2);
    },
    [setUser]
  );

  const updatePage = useCallback(
    (page2: Pages) => {
      setPage(page2);
    },
    [setPage]
  );

  const registerScreen = useMemo(() => {
    return (
      <RegisterScreen user={user} setUser={updateUser} setPage={updatePage} />
    );
  }, [user, updateUser, updatePage]);

  const loginScreen = useMemo(() => {
    return (
      <LoginScreen user={user} updateUser={updateUser} setPage={updatePage} />
    );
  }, [user, updateUser, updatePage, page]);

  const homeScreen = useMemo(() => {
    return (
      <HomeScreen
        user={user}
        setUser={updateUser}
        page={page}
        setPage={updatePage}
      ></HomeScreen>
    );
  }, [page, user, updatePage, updateUser]);

  const headerMenu = useMemo(() => {
    return <HeaderMenu user={user} setUser={updateUser} setPage={setPage} />;
  }, [user, updateUser, setPage]);

  return (
    <ChakraProvider>
      {headerMenu}
      {page === Pages.register ? registerScreen : null}
      {page === Pages.login ? loginScreen : null}
      {page === Pages.home ? homeScreen : null}
    </ChakraProvider>
  );
}

export default App;
