import { useCallback } from "react";
import { Pages, serverUrl, User } from "../constants";
import axios from "axios";
import {
  Flex,
  Heading,
  Spacer,
  Button,
  ButtonGroup,
} from "@chakra-ui/react";

function HeaderMenu({
  user,
  updatePage,
}: {
  user: User;
  updatePage: React.Dispatch<React.SetStateAction<Pages>>;
}) {
  const register = useCallback(() => {
    updatePage(Pages.register);
  }, []);

  const signup = useCallback(() => {
    updatePage(Pages.signup);
  }, []);

  const logOut = useCallback(() => {
    axios.get(`${serverUrl}/Account/LogOut`);
    user.isSignIn = false;
    user.userName = "";
    user.role = "";
  }, [user]);

  return (
    <Flex as="header" padding="1rem" bg="#182681" color="white">
      <Heading size="lg">
        {user.isSignIn ? "Hello " + user.userName : "Events"}
      </Heading>
      <Spacer />
      {user.isSignIn ? (
        <Button onClick={logOut}>LogOut</Button>
      ) : (
        <ButtonGroup>
          <Button onClick={signup}>SignUp</Button>
          <Button onClick={register}>Register</Button>
        </ButtonGroup>
      )}
    </Flex>
  );
}

export default HeaderMenu;
