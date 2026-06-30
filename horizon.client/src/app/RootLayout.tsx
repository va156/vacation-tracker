import { Outlet } from "react-router-dom";
import Header from "../shared/components/Header/Header";

export default function RootLayout() {
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-grow">
                <Outlet /> 
            </main>
        </div>
    );
}