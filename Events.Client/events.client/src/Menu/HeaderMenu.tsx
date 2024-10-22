import { useCallback } from "react";
import { serverUrl, User } from "../constants";
import axios from "axios";
import { Flex, Heading, Spacer, Button, ButtonGroup } from "@chakra-ui/react";

function HeaderMenu({ user, navigation }: { user: User; navigation: User }) {
  const logOut = useCallback(() => {
    axios.get(`${serverUrl}/Account/LogOut`).then((response) => {
      if (response.status === 200) {
        user.isSignin = false;
        user.jwtToken = "";
        user.refreshToken = "";
      }
    });
  }, [user]);

  return (
    <Flex as="header" padding="1rem" bg="#182681" color="white">
      <Heading size="lg">
        {user.isSignin ? "Hello " + user.jwtToken : "Events"}
      </Heading>
      <Spacer />
      {user.isSignin ? (
        <Button onClick={logOut}>LogOut</Button>
      ) : (
        <ButtonGroup></ButtonGroup>
      )}
    </Flex>
  );
}

export default HeaderMenu;
