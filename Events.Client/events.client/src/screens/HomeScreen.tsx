import React, { useCallback, useEffect, useState } from "react";
import { GetEventResponseDto, Pages, serverUrl, User } from "../constants";
import axios from "axios";
import { refreshToken } from "../requests/refreshToken";
import {
  Card,
  CardBody,
  Heading,
  Image,
  Stack,
  StackItem,
  Text,
} from "@chakra-ui/react";
function HomeScreen({
  user,
  setUser,
  page,
  setPage,
}: {
  user: User;
  setUser: (user: User) => void;
  page: Pages;
  setPage: (page: Pages) => void;
}) {
  const [pageIndex, setPageIndex] = useState<number>(1);
  const [events, setEvents] = useState<GetEventResponseDto[]>([]);

  useEffect(() => {
    axios.get(`${serverUrl}/Event/List?page=${pageIndex}`).then((response) => {
      if (response.status === 200) {
        const responseDto = response.data as GetEventResponseDto[];
        setEvents(responseDto);
      }
      if (response.status === 401) {
        const userDto = refreshToken(setUser);
        if (userDto.isLogedIn === false) {
          setPage(Pages.login);
        }
        axios
          .get(`${serverUrl}/Event/List?page=${pageIndex}`)
          .then((response) => {
            if (response.status === 200) {
              const responseDto = response.data as GetEventResponseDto[];
              setEvents(responseDto);
            }
          });
      }
    });
  }, [pageIndex]);

  const createEventView = useCallback(
    (responseDto: GetEventResponseDto) => {
      return (
        <Card>
          <CardBody>
            <Stack mt="6" spacing="3">
              <Heading size="md">{responseDto.title}</Heading>
              <Text>{responseDto.description}</Text>
              <Text color="blue.600" fontSize="2xl">
                Place: {responseDto.place};
              </Text>
            </Stack>
            <Image
              src={`${serverUrl}/EventImages/${responseDto.eventImageUrl}`}
            ></Image>
          </CardBody>
        </Card>
      );
    },
    [events]
  );

  return <Stack spacing="5px">{events.map((e) => createEventView(e))}</Stack>;
}

export default HomeScreen;
