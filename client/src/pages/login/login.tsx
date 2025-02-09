import { useNavigate } from "react-router-dom"

import { LoginForm } from "@/components/form/login/login-form.tsx"

import styles from "./styles/login.module.css"

export function Login() {
    const navigate = useNavigate()

    return (
        <div className={styles.container}>
            <div className={styles.formWrapper}>
                <div className={styles.header}>
                    <h1 className={styles.title}>Welcome Back!</h1>
                    <p className={styles.subtitle}>Please sign in to continue</p>
                </div>

                <LoginForm />

                <div className={styles.signupLink}>
                    <span className={styles.signupText}>
                        Don&apos;t have an account?{' '}
                        <button 
                            className={styles.signupButton}
                            onClick={() => navigate('/signup-options')}
                        >
                            Sign up
                        </button>
                    </span>
                </div>
            </div>
        </div>
    )
}