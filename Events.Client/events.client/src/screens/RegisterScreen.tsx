import { useCallback, useMemo, useState } from "react";
import { RegisterDTO, serverUrl, User } from "../constants";
import axios from "axios";
import {
  Card,
  Input,
  InputGroup,
  InputRightElement,
  Button,
  Text,
} from "@chakra-ui/react";

function RegisterScreen({ user }: { user: User }) {
  const [email, setEmail] = useState<string | null>(null);
  const [password, setPassword] = useState<string | null>(null);
  const [userName, setUserName] = useState<string | null>(null);

  const [showPassword, setShowPassword] = useState(false);
  const handleShowPasswordClick = () => setShowPassword(!showPassword);

  const [loginResponseDto, setLoginResponseDto] = useState(null);

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
          user = JSON.parse(response.data);
        }
      });
  }, [registrationRequestDto]);

  return (
    <Card>
      <InputGroup size="md">
        <Input
          placeholder="Username"
          onInput={(e) => setUserName(e.target.value)}
        />
      </InputGroup>
      <InputGroup size="md">
        <Input placeholder="Email" onInput={(e) => setEmail(e.target.value)} />
      </InputGroup>
      <InputGroup size="md">
        <Input
          pr="4.5rem"
          type={showPassword ? "text" : "password"}
          placeholder="Enter password"
          onInput={(e) => setPassword(e.target.value)}
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
