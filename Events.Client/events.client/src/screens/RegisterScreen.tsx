import { useCallback, useMemo, useState } from "react";
import { Pages, RegisterDTO, serverUrl, User } from "../constants";
import axios from "axios";
import {
  Card,
  Input,
  InputGroup,
  InputRightElement,
  Button,
  Text,
} from "@chakra-ui/react";

function RegisterScreen({
  user,
  setUser,
  setPage,
}: {
  user: User;
  setUser: (user: User) => void;
  setPage: (page: Pages) => void;
}) {
  const [email, setEmail] = useState<string | null>(null);
  const [password, setPassword] = useState<string | null>(null);
  const [userName, setUserName] = useState<string | null>(null);

  const [showPassword, setShowPassword] = useState(false);
  const handleShowPasswordClick = () => setShowPassword(!showPassword);

  const registrationRequestDto = useMemo<RegisterDTO>(() => {
    return {
      email: email ?? "",
      password: password ?? "",
      userName: userName ?? "",
    };
  }, [userName, email, password]);

  const register = useCallback(() => {
    axios
      .post(`${serverUrl}/RegisteredAccount/Add`, registrationRequestDto)
      .then((response) => {
        if (response.status === 200) {
          user = response.data as User;
          if (user != null) {
            setUser(user);
            setPage(Pages.home);
          }
        }
      });
  }, [registrationRequestDto]);

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
          placeholder="Email"
          onInput={(e: React.FormEvent<HTMLInputElement>) =>
            setEmail(e.currentTarget.value)
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
        onClick={() => register()}
      >
        <Text>Register</Text>
      </Button>
    </Card>
  );
}

export default RegisterScreen;
