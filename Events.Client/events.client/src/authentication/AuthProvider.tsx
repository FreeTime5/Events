import {
  createContext,
  useContext,
  useEffect,
  useLayoutEffect,
  useState,
} from "react";

import axios, { AxiosResponse } from "axios";
import { LoginDto, serverUrl, User } from "../constants";

const AuthContext = createContext(undefined);

export const useAuth = () => {
  const authContext = useContext(AuthContext);

  if (authContext === undefined) {
    throw new Error("useAuth must be with AuthProvider");
  }

  return authContext;
};

type Props = {
  children: React.ReactNode;
};

function AuthProvider({ children }: Props) {
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [refreshToken, setRefreshToken] = useState<string | null>(null);

  useEffect(() => {
    const fetchIsLogin = async () => {
      await axios
        .get(`${serverUrl}/LogIn`)
        .then((response) => {
          const userResponse = response as AxiosResponse<User>;
          setAccessToken(userResponse.data.jwtToken);
          setRefreshToken(userResponse.data.refreshToken);
        })
        .catch(() => {
          setAccessToken(null);
          setRefreshToken(null);
        });
    };
    fetchIsLogin();
  }, []);

  useLayoutEffect(() => {
    const authInterceptor = axios.interceptors.request.use((config) => {
      config.headers!.Authorization =
        !config.retry && accessToken
          ? `Bearer ${accessToken}`
          : config.headers!.Authorization;
      return config;
    });
    return () => {
      axios.interceptors.request.eject(authInterceptor);
    };
  }, [accessToken, refreshToken]);

  useLayoutEffect(() => {
    const refreshInterceptor = axios.interceptors.response.use(
      (response) => response,
      async (error) => {
        const request = error.config;
        if (error.response.status === 401) {
          await axios
            .get(`${serverUrl}/LogIn/RefreshToken`)
            .then((respnse) => {
              const userResponse = respnse as AxiosResponse<User>;
              setAccessToken(userResponse.data.jwtToken);
              setRefreshToken(userResponse.data.refreshToken);
              request.headers.Authorization = `Bearer ${accessToken}`;
              request.retry = true;
              return axios(request);
            })
            .catch(() => {
              setAccessToken(null);
              setRefreshToken(null);
            });
        }
        return Promise.reject(error);
      }
    );
    return () => {
      axios.interceptors.response.eject(refreshInterceptor);
    };
  }, [accessToken, refreshToken]);

  return (
    <AuthContext.Provider value={undefined}>{children}</AuthContext.Provider>
  );
}

export default AuthProvider;
