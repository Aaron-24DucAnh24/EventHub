import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from "@mui/material";
import React, { useEffect, useState } from "react";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { ISeatEvent } from "../Dto/ISeat";
import { IEvent } from "../Dto/IEvent";
import { formatDateEventDetail } from "../utils/convertDateEvent";
import { BiTimeFive, BiSolidMap } from "react-icons/bi";
import api from "../api";

interface Props {
  seatsList: ISeatEvent[] | undefined;
  setSeatsList: (seatsList: ISeatEvent[]) => void;
  event: IEvent | undefined;
}

const theme = createTheme({
  palette: {
    primary: {
      main: "#5669FF",
    },
  },
});

const EventDescription = ({ seatsList, setSeatsList, event }: Props) => {
  console.log('event: ', event);
  
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [openInfo, setOpenInfo] = useState(false);
  const [openConfirmationCode, setOpenConfirmationCode] = useState(false);
  const handleCloseInfo = () => setOpenInfo(false);
  const handleCloseConfirmationCode = () => setOpenConfirmationCode(false);
  const [seatsPicking, setSeatsPicking] = useState<ISeatEvent[]>([]);
  const [seatsNumber, setSeatsNumber] = useState<string>("");
  const [isSubmitInfo, setIsSubmitInfo] = useState<boolean>(false);

  //Form data
  const [fullName, setFullName] = useState<string>("");
  const [phoneNumber, setPhoneNumber] = useState<string>("");
  const [email, setEmail] = useState<string>("");

  const handleClickPaymentBtn = () => {
    setOpenInfo(true);
  };

  const handleSubmitDialogInfo = async () => {
    try {
      setIsLoading(true);
      const response = await api.get("/Invoice", {
        params: {
          email: email,
          fullName: fullName,
        },
        headers: {
          "Content-Type": "text/plain",
          "ngrok-skip-browser-warning": "true",
        },
      });
      console.log(response);
      setIsSubmitInfo(true);
      setOpenConfirmationCode(true);
      handleCloseInfo();
    } catch (error) {
      console.error("Error fetching data:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const hanldeSubmitConfirmationCode = async () => {
    try {
      setIsLoading(true);
      const config = {
        headers: {
          "ngrok-skip-browser-warning": "true",
        },
      };
      if (!event?.id) return;
      let postData = {
        fullName: fullName,
        mail: email,
        phone: phoneNumber,
        eventId: event?.id,
        seatIds: seatsPicking.map((seat) => seat.seatId.toLowerCase()),
      };
      const response = await api.post("/Invoice", postData, config);
      if (response && seatsList) {
        let seatsUpdate = [...seatsList];
        seatsUpdate.map((seat) => {
          if (seat.seatStatus === -1) {
            seat.seatStatus = 1;
          }
        });
        setSeatsList(seatsUpdate);
      }
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoading(false);
      handleCloseConfirmationCode();
    }
  };

  const fetchPaymentPicking = () => {
    if (seatsList === undefined) return;
    const seats = seatsList.filter((seat) => seat.seatStatus === -1);
    setSeatsNumber(seats.map((seat) => seat.seatId).join(", "));
    setSeatsPicking(seats);
  };
  useEffect(() => {
    fetchPaymentPicking();
  }, [seatsList]);

  //Calculate total price
  const totalTickets = seatsPicking.length;
  const totalPrice = seatsPicking.reduce((total, seat) => {
    return total + seat.price;
  }, 0);

  return (
    <div>
      {event && (
        <div className="mt-[80px] flex gap-[50px] p-[20px] rounded-[20px] border-[#ccc] border-solid border-[4px] min-h-[230px]">
          <div className="flex flex-row flex-[7] gap-[16px]">
            <div>
              <img
                src={event.image}
                alt="event"
                className="w-[200px] h-full object-cover"
              />
            </div>
            <div className="flex flex-col justify-between">
              <h3 className="my-0 text-[24px]">{event.title}</h3>
              <div className="flex flex-col">
                <p className="my-0 text-[18px] font-[700] flex items-center mb-[8px] gap-[10px]">
                  <span className="flex items-center">
                    <BiTimeFive className="text-[#999]" />
                  </span>
                  <span>
                    {`${formatDateEventDetail(event.date)} (${event.duration})`}
                  </span>
                </p>
                <div className="flex items-center gap-[10px]">
                  <span className="flex items-center">
                    <BiSolidMap className="text-[#999] text-[20px]" />
                  </span>
                  <div className="flex flex-col gap-[6px]">
                    <p className="my-0 text-[18px] font-[700]">
                      {event.stageName}
                    </p>
                    <p className="my-0">{event.location}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="flex flex-[2] flex-col">
            <div className="flex flex-col gap-[16px]">
              {seatsPicking.map((seat) => (
                <div key={seat.seatId} className="flex justify-between">
                  <p className="my-0">
                    {seat.seat.type === 0
                      ? `Standard ${`(${seat.seatId})`}`
                      : seat.seat.type === 1
                      ? `VIP ${`(${seat.seatId})`}`
                      : `Sweet Box ${`(${seat.seatId})`}`}
                  </p>
                  <p className="my-0">{seat.price.toLocaleString()} x 1</p>
                </div>
              ))}
            </div>
            {totalTickets > 0 ? (
              <div className="flex justify-between my-[16px]">
                <h3 className="my-0">Total</h3>
                <h3 className="my-0">{`${totalPrice.toLocaleString()} VND`}</h3>
              </div>
            ) : (
              ""
            )}
            <p className="my-0 mb-[16px]">
              (Number of tickets booked: {totalTickets}/3)
            </p>
            <ThemeProvider theme={theme}>
              {totalTickets > 0 ? (
                <Button
                  variant="contained"
                  component="label"
                  className="rounded-[10px]"
                  onClick={handleClickPaymentBtn}
                >
                  Payment
                </Button>
              ) : (
                ""
              )}
              <Dialog open={openInfo} onClose={handleCloseInfo}>
                <DialogTitle
                  className="text-main"
                  style={{ color: "#5669FF", fontSize: "24px" }}
                >
                  Confirmation Payment
                </DialogTitle>
                <DialogContent
                  style={{
                    height: "100%",
                    position: "relative",
                    display: "flex",
                    flexDirection: "column",
                  }}
                >
                  <DialogContentText>
                    Please provide the required information for payment
                    confirmation.
                  </DialogContentText>
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "end",
                    }}
                  >
                    <p style={{ flex: 2, margin: "0", fontSize: "18px" }}>
                      Full name:{" "}
                    </p>
                    <TextField
                      autoFocus
                      margin="dense"
                      id="name"
                      label="Full name"
                      type="text"
                      fullWidth
                      variant="standard"
                      style={{ flex: 4, margin: "0" }}
                      onChange={(e) => setFullName(e.target.value)}
                    />
                  </div>
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "end",
                      marginTop: "12px",
                    }}
                  >
                    <p style={{ flex: 2, margin: "0", fontSize: "18px" }}>
                      Phone number:{" "}
                    </p>
                    <TextField
                      autoFocus
                      margin="dense"
                      id="phone"
                      label="Phone number"
                      type="number"
                      fullWidth
                      variant="standard"
                      style={{ flex: 4, margin: "0" }}
                      onChange={(e) => setPhoneNumber(e.target.value)}
                    />
                  </div>
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "end",
                      marginTop: "12px",
                    }}
                  >
                    <p style={{ flex: 2, margin: "0", fontSize: "18px" }}>
                      Email:{" "}
                    </p>
                    <TextField
                      autoFocus
                      margin="dense"
                      id="phone"
                      label="Email"
                      type="email"
                      fullWidth
                      variant="standard"
                      style={{ flex: 4, margin: "0" }}
                      onChange={(e) => setEmail(e.target.value)}
                    />
                  </div>
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "end",
                      marginTop: "12px",
                    }}
                  >
                    <p style={{ flex: 2, margin: "0", fontSize: "18px" }}>
                      Event name:{" "}
                    </p>
                    <TextField
                      autoFocus
                      margin="dense"
                      id="phone"
                      label="Event name"
                      type="text"
                      defaultValue={event.title}
                      InputProps={{
                        readOnly: true,
                      }}
                      fullWidth
                      variant="standard"
                      style={{ flex: 4, margin: "0" }}
                    />
                  </div>
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "end",
                      marginTop: "12px",
                    }}
                  >
                    <p style={{ flex: 2, margin: "0", fontSize: "18px" }}>
                      Seats:{" "}
                    </p>
                    <TextField
                      autoFocus
                      margin="dense"
                      id="text"
                      label="Seats"
                      type="text"
                      defaultValue={seatsNumber}
                      InputProps={{
                        readOnly: true,
                      }}
                      fullWidth
                      variant="standard"
                      style={{ flex: 4, margin: "0" }}
                    />
                  </div>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleCloseInfo}>Cancel</Button>
                  <Button onClick={handleSubmitDialogInfo}>Submit</Button>
                </DialogActions>
              </Dialog>

              <Dialog
                open={openConfirmationCode}
                onClose={handleCloseConfirmationCode}
              >
                <DialogTitle
                  className="text-main"
                  style={{ color: "#5669FF", fontSize: "24px" }}
                >
                  Email Validation
                </DialogTitle>
                <DialogContent
                  style={{
                    height: "100%",
                    position: "relative",
                    display: "flex",
                    flexDirection: "column",
                  }}
                >
                  <DialogContentText>
                    {`We just sent a confirmation code over to ${email}. Please enter the code below to confirm your payment.`}
                  </DialogContentText>
                  <div
                    style={{
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "end",
                    }}
                  >
                    <p
                      style={{
                        flex: 2,
                        margin: "0",
                        fontSize: "18px",
                        width: "300px",
                        display: "block",
                      }}
                    >
                      Confirmation code:{" "}
                    </p>
                    <TextField
                      autoFocus
                      margin="dense"
                      id="name"
                      label="Enter code"
                      type="number"
                      fullWidth
                      variant="standard"
                      style={{ flex: 4, margin: "0" }}
                    />
                  </div>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleCloseConfirmationCode}>Cancel</Button>
                  <Button onClick={hanldeSubmitConfirmationCode}>Submit</Button>
                </DialogActions>
              </Dialog>
            </ThemeProvider>
          </div>
        </div>
      )}
    </div>
  );
};

export default EventDescription;
