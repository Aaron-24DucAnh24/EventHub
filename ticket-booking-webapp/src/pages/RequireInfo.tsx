import { Button, Container, TextField, ThemeProvider } from "@mui/material";
import React, { useState } from "react";
import { createTheme } from "@mui/material/styles";
import { useEmailStore } from "../store/email";
import shallow from "zustand/shallow";
import { useNavigate } from "react-router-dom";

const theme = createTheme({
  palette: {
    primary: {
      main: "#5669FF",
    },
  },
});

const RequireInfo = () => {
  const navigate = useNavigate();
  //Zustand
  const [setEmailStore] = useEmailStore(
    (state: any) => [state.setEmailStore],
    shallow
  );

  const [email, setEmail] = useState<string>("");
  const [fullName, setFullName] = useState<string>("");
  const [phoneNumber, setPhoneNumber] = useState<string>("");

  const handleSubmitInfo = () => {
    if (!email) {
      alert("Email is required");
      return;
    }
    if (!fullName) {
      alert("Full name is required");
      return;
    }
    if (!phoneNumber) {
      alert("Phone number is required");
      return;
    }
    setEmailStore(email);
    navigate(`/my-booking/${email}`);
  };
  return (
    <Container className="mt-[80px] mb-[80px] flex gap-[40px] flex-col w-[40%]">
      <h3 className="m-0 mb-[60px] text-[32px] leading-[40px] text-main">
        To view My Booking, please provide your information.
      </h3>
      <div className="flex items-center justify-between">
        <p className="my-0 text-[20px]">Email</p>
        <ThemeProvider theme={theme}>
          <TextField
            label="Email"
            variant="outlined"
            style={{ width: "500px" }}
            type="text"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </ThemeProvider>
      </div>
      <div className="flex items-center justify-between">
        <p className="my-0 text-[20px]">Full Name</p>
        <ThemeProvider theme={theme}>
          <TextField
            label="Full name"
            variant="outlined"
            style={{ width: "500px" }}
            type="text"
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
          />
        </ThemeProvider>
      </div>
      <div className="flex items-center justify-between">
        <p className="my-0 text-[20px]">Phone Number</p>
        <ThemeProvider theme={theme}>
          <TextField
            label="Phone number"
            variant="outlined"
            style={{ width: "500px" }}
            type="text"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
          />
        </ThemeProvider>
      </div>
      <div className="mt-[40px] flex justify-center">
        <ThemeProvider theme={theme}>
          <Button
            variant="contained"
            className="flex w-[400px] text-[16px] py-[10px] rounded-[10px]"
            onClick={handleSubmitInfo}
          >
            Submit
          </Button>
        </ThemeProvider>
      </div>
    </Container>
  );
};

export default RequireInfo;
