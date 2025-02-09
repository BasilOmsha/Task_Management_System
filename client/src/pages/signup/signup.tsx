import { useNavigate } from "react-router-dom"

import { SignupForm } from "@/components/form/signup/signup-form.tsx"

import styles from "./signup.module.css"

export default function Signup() {
    const navigate = useNavigate()

    return (
        <div className={styles.container}>
            <div className={styles.card}>
                <div className={styles.header}>
                    <h1 className={styles.title}>Create your account</h1>
                    <p className={styles.subtitle}>Enter your details below to get started</p>
                </div>
                <SignupForm />
            </div>
            <div className={styles.signupLink}>
                <span className={styles.signupText}>
                    Already have an account?{' '}
                    <button 
                        className={styles.signupButton}
                        onClick={() => navigate('/login')}
                    >
                        Log in
                    </button>
                </span>
            </div>
        </div>
    )
}