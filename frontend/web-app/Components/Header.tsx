import Link from 'next/link'
import React from 'react'
import { GiHamburgerMenu } from 'react-icons/gi'
import { IoCarSportOutline } from 'react-icons/io5'

const Header = () => {
  return (
    <header className='w-full px-5 py-3 border-b-2 bg-white'>
        <nav className='flex justify-between'>
            <Link href={""} className='flex justify-center items-center gap-2'>
                <IoCarSportOutline size={36}/>
                <p className='text-2xl'>Autocar</p>
            </Link>
            <Link href={""}>Nav Items</Link>
            <GiHamburgerMenu className='cursor-pointer'size={28}/>
        </nav>
    </header>
  ) 
}

export default Header