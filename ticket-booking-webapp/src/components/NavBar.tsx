import { Button, TextField } from "@mui/material";
import { Link } from "react-router-dom";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { useSearchStore } from "../store/search";
import { shallow } from "zustand/shallow";
import { useEmailStore } from "../store/email";

const theme = createTheme({
  palette: {
    primary: {
      main: "#5669FF",
    },
  },
});
const NavBar = () => {
  const [searchString, setSearchString] = useSearchStore(
    (state) => [state.searchString, state.setSearchString],
    shallow
  );

  const [emailStore] = useEmailStore((state) => [state.emailStore], shallow);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchString(event.target.value);
  };

  return (
    <div className="flex justify-between items-center px-[40px] py-[20px] border-0 border-b-[1px] border-b-[#ccc] border-solid bg-[#fff]">
      <div className="flex gap-[24px] items-center ">
        <Link to="/">
          <div className="cursor-pointer">
            <img src={require("../assets/icons/Logo.png")} alt="logo" />
          </div>
        </Link>
        <div className="">
          <ThemeProvider theme={theme}>
            <TextField
              label="Search event..."
              variant="outlined"
              className="w-[360px] rounded-[10px]"
              value={searchString}
              onChange={handleSearchChange}
            />
          </ThemeProvider>
        </div>
      </div>
      <div className="flex gap-[24px]">
        <Link to="/create-event">
          <Button
            variant="outlined"
            className="rounded-[20px] border-x-main border-y-main text-main px-[24px] py-[6px]"
          >
            Create event
          </Button>
        </Link>
        <Link to={emailStore ? `/my-booking/${emailStore}` : "require-info"}>
          <Button
            variant="outlined"
            className="rounded-[20px] border-x-main border-y-main text-main px-[24px] py-[6px]"
          >
            My Booking
          </Button>
        </Link>
        <Link to="/unpublished-event">
          <Button
            variant="contained"
            className="rounded-[20px] bg-main text-[#fff] px-[24px] py-[6px]"
          >
            Unpublished Event
          </Button>
        </Link>
        {/* <Link to="/statistic">
          <Button
            variant="contained"
            className="rounded-[20px] bg-main text-[#fff] px-[24px] py-[6px]"
          >
            Statistic
          </Button>
        </Link> */}
      </div>
    </div>
  );
};

export default NavBar;
