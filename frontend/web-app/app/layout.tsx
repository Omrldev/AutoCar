import type { Metadata } from "next";
import localFont from "next/font/local";
import "./globals.css";
import { Rubik } from "next/font/google";
import Header from "@/Components/Header";
import Footer from "@/Components/Footer";

const geistSans = localFont({
  src: "./fonts/GeistVF.woff",
  variable: "--font-geist-sans",
  weight: "100 900",
});
const geistMono = localFont({
  src: "./fonts/GeistMonoVF.woff",
  variable: "--font-geist-mono",
  weight: "100 900",
});
const rubik = Rubik({subsets: ["latin"]});

export const metadata: Metadata = {
  title: "AutoCar | Omar Ledesma",
  description: "Auction Car project",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Header />
        <main className={`${geistMono.className} relative`}>{children}</main>
        <Footer />
      </body>
    </html>
  );
}
