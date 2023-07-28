import { Outlet } from "react-router-dom";
import Footer from "./Footer";
import NavBar from "./NavBar";

const Layout = () => {
  return (
    <div className="h-full min-h-[100vh] flex flex-col justify-between">
      <NavBar />
      <Outlet />
      <Footer />
    </div>
  );
};

export default Layout;
