import { useCallback, useMemo } from "react";
import { Pages, serverUrl, User } from "../constants";
import axios, { AxiosResponse } from "axios";
import { Flex, Heading, Spacer, Button, ButtonGroup } from "@chakra-ui/react";
import { refreshToken } from "../requests/refreshToken";

function HeaderMenu({
  user,
  setUser,
  setPage,
}: {
  user: User;
  setUser: (user: User) => void;
  setPage: React.Dispatch<React.SetStateAction<Pages>>;
}) {
  const logout = useCallback(() => {
    var requestResponse = null as AxiosResponse<any, any> | null;
    axios
      .get(`${serverUrl}/LogIn`)
      .catch(() => {
        refreshToken(setUser);
      })
      .finally(() => {
        axios.get(`${serverUrl}/LogIn/LogOut`).then((response) => {
          if (response.status == 200) {
            user.isLogedIn = false;
            user.jwtToken = "";
            user.refreshToken = "";
            setUser(user);
          }
          setPage(Pages.login);
        });
      });
    return requestResponse;
  }, [user, setUser, setPage]);

  const login = useCallback(() => {
    setPage(Pages.login);
  }, [setPage]);

  const register = useCallback(() => {
    setPage(Pages.register);
  }, [setPage]);

  const headingText = useMemo(() => {
    return user.isLogedIn ? "Hello " + user.jwtToken[0] : "Events";
  }, [user]);

  const menuButtons = useMemo(() => {
    return user.isLogedIn ? (
      <Button onClick={logout}>LogOut</Button>
    ) : (
      <ButtonGroup>
        <Button onClick={() => login()}>Login</Button>
        <Button onClick={() => register()}>Register</Button>
      </ButtonGroup>
    );
  }, [user]);

  const HeaderMenu = useMemo(() => {
    return (
      <Flex as="header" padding="1rem" bg="#182681" color="white">
        <Heading size="md">{headingText}</Heading>
        <Spacer />
        {menuButtons}
      </Flex>
    );
  }, [user]);

  return <>{HeaderMenu}</>;
}

export default HeaderMenu;
