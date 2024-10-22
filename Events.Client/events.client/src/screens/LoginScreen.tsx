import { useCallback, useMemo, useState } from "react";
import { LoginDto, Pages, serverUrl, User } from "../constants";
import Cookies from "js-cookie";
import axios from "axios";
import {
  Button,
  Card,
  cookieStorageManager,
  Input,
  InputGroup,
  InputRightElement,
  Text,
} from "@chakra-ui/react";

function LoginScreen({
  user,
  updateUser,
  setPage,
}: {
  user: User;
  updateUser: (user: User) => void;
  setPage: (page: Pages) => void;
}) {
  axios.defaults.withCredentials = true;
  const [userName, setUserName] = useState<string | null>(null);
  const [password, setPassword] = useState<string | null>(null);

  const [showPassword, setShowPassword] = useState(false);
  const handleShowPasswordClick = () => setShowPassword(!showPassword);

  const loginDto = useMemo<LoginDto>(() => {
    return {
      userName: userName ?? "",
      password: password ?? "",
    };
  }, [userName, password]);

  const login = useCallback(() => {
    axios.post(`${serverUrl}/LogIn`, loginDto).then((response) => {
      if (response.status === 200) {
        const userDto = response.data as User;
        if (userDto != null) {
          updateUser(userDto);
          setPage(Pages.home);
        }
      }
    });
  }, [loginDto, updateUser]);

  return (
    <Card>
      <InputGroup size="md">
        <Input
          placeholder="Username"
          onInput={(e: React.FormEvent<HTMLInputElement>) =>
            setUserName(e.currentTarget.value)
          }
        />
      </InputGroup>
      <InputGroup size="md">
        <Input
          pr="4.5rem"
          type={showPassword ? "text" : "password"}
          placeholder="Enter password"
          onInput={(e: React.FormEvent<HTMLInputElement>) =>
            setPassword(e.currentTarget.value)
          }
        />
        <InputRightElement width="4.5rem">
          <Button h="1.75rem" size="sm" onClick={handleShowPasswordClick}>
            {showPassword ? "Hide" : "Show"}
          </Button>
        </InputRightElement>
      </InputGroup>
      <Button
        colorScheme="teal"
        variant="outline"
        size="md"
        onClick={() => login()}
      >
        <Text>Login</Text>
      </Button>
    </Card>
  );
}

export default LoginScreen;
